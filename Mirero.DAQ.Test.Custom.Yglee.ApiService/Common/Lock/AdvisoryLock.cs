using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Npgsql;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Lock
{
    public class AdvisoryLock
    {
        private const int AlreadyHeldReturnCode = 103;

        private readonly AdvisoryLockKey _key;
        private readonly DbContext _context;
        private readonly bool _isShared;


        public AdvisoryLock(AdvisoryLockKey key, DbContext context , bool isShared = false)
        {
            _key = key;
            _context = context;
            _isShared = isShared;
        }

        public async ValueTask<object?> TryAcquire(TimeSpan timeout = new TimeSpan())
        {
            var conn = _context.Database.GetDbConnection();
            await _context.Database.OpenConnectionAsync();

            await using var acquireCommand = this.CreateAcquireCommand(conn, timeout);

            var acquireCommandResult = 0;
            try
            {
                acquireCommandResult = (int)(await acquireCommand.ExecuteScalarAsync() ?? 0);
            }
            catch (Exception ex)
            {
                if (ex is PostgresException postgresException)
                {
                    switch (postgresException.SqlState)
                    {
                        // lock_timeout error code from https://www.postgresql.org/docs/10/errcodes-appendix.html
                        case "55P03":
                            return null;
                        // deadlock_detected error code from https://www.postgresql.org/docs/10/errcodes-appendix.html
                        case "40P01":
                            throw new InvalidOperationException($"The request for the distributed lock failed with exit code '{postgresException.SqlState}' (deadlock_detected)", ex);
                    }
                }
            }

            switch (acquireCommandResult)
            {
                case 0: return null;
                case 1: return new object();
                case AlreadyHeldReturnCode:
                    if (timeout.Milliseconds == 0) { return null; }
                    if (timeout.Milliseconds == -1) { throw new InvalidOperationException("Attempted to acquire a lock that is already held on the same connection"); }
                    await Task.Delay(timeout).ConfigureAwait(false);
                    return null;
                default:
                    throw new InvalidOperationException($"Unexpected return code {acquireCommandResult}");
            }
        }

        private DbCommand CreateAcquireCommand(DbConnection connection, TimeSpan timeout)
        {
            var cmd = connection.CreateCommand();

            var cmdText = new StringBuilder();

            cmdText.AppendLine("SET LOCAL statement_timeout = 0;");

            cmdText.AppendLine($"SET LOCAL lock_timeout = {timeout.Milliseconds};");

            cmdText.Append($@"
                    SELECT 
                        CASE WHEN EXISTS(
                            SELECT * 
                            FROM pg_catalog.pg_locks l
                            JOIN pg_catalog.pg_database d
                                ON d.oid = l.database
                            WHERE l.locktype = 'advisory' 
                                AND {AddPGLocksFilterParametersAndGetFilterExpression(cmd, _key)} 
                                AND l.pid = pg_catalog.pg_backend_pid() 
                                AND d.datname = pg_catalog.current_database()
                        ) 
                            THEN {AlreadyHeldReturnCode}
                        ELSE
                            "
            );

            AppendAcquireFunctionCall();

            cmdText.AppendLine().Append("END");

            cmdText.Append(" AS result");

            cmd.CommandText = cmdText.ToString();
            cmd.CommandTimeout = timeout.Milliseconds;

            return cmd;

            void AppendAcquireFunctionCall()
            {
                // creates an expression like
                // (SELECT 1 FROM (SELECT pg_advisory_lock(@key)) f)
                var isTry = timeout.Milliseconds == 0;
                if (!isTry) { cmdText.Append("(SELECT 1 FROM (SELECT "); }
                cmdText.Append("pg_catalog.pg");
                if (isTry) { cmdText.Append("_try"); }
                cmdText.Append("_advisory");
                cmdText.Append("_lock");
                if (this._isShared) { cmdText.Append("_shared"); }
                cmdText.Append('(').Append(AddKeyParametersAndGetKeyArguments(cmd, _key)).Append(')');
                if (isTry) { cmdText.Append("::int"); }
                else { cmdText.Append(") f)"); }
            }
        }

        private string AddPGLocksFilterParametersAndGetFilterExpression(DbCommand command, AdvisoryLockKey key)
        {
            string classIdParameter, objIdParameter, objSubId = "1";

            var (keyUpper32, keyLower32) = key.Keys;
            AddCommandParameter(command, classIdParameter = "keyUpper32", keyUpper32, DbType.Int32);
            AddCommandParameter(command, objIdParameter = "keyLower32", keyLower32, DbType.Int32);

            return $"(l.classid = @{classIdParameter} AND l.objid = @{objIdParameter} AND l.objsubid = {objSubId})";
        }

        private static string AddKeyParametersAndGetKeyArguments(DbCommand command, AdvisoryLockKey key)
        {
            AddCommandParameter(command, "key", key.Key, DbType.Int64);
            return "@key";
        }

        private static void AddCommandParameter(DbCommand command, string name, object value, DbType? dbType)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            if (dbType != null) parameter.DbType = dbType.Value;
            command.Parameters.Add(parameter);
        }
    }
}
