using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Lock
{
    public class AdvisoryLock
    {
        private readonly AdvisoryLockKey _key;
        private readonly bool _isShared;


        public AdvisoryLock(AdvisoryLockKey key, bool isShared = false)
        {
            _key = key;
            _isShared = isShared;
        }

        public async ValueTask<object?> TryAcquire(DbContext context, TimeSpan timeout)
        {
            await using var conn = context.Database.GetDbConnection();
            await context.Database.OpenConnectionAsync();

            using var acquireCommand = this.CreateAcquireCommand(conn);

            int acquireCommandResult = 0;
            try
            {
                acquireCommandResult = (int)await acquireCommand.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                
            }

            return acquireCommandResult;
        }

        private DbCommand CreateAcquireCommand(DbConnection connection, TimeSpan timeout = new TimeSpan())
        {
            var cmd = connection.CreateCommand();

            var cmdText = new StringBuilder();

            cmdText.AppendLine("SET LOCAL statement_timeout = 0;");

            cmdText.AppendLine($"SET LOCAL lock_timeout = {timeout.Milliseconds};");

            cmdText.Append("SELECT");

            cmdText.Append(" AS result");

            cmd.CommandText = cmdText.ToString();
            cmd.CommandTimeout = timeout.Milliseconds;

            return cmd;

            void AppendAcquireFunctionCall()
            {
                // creates an expression like
                // pg_try_advisory_lock(@key1, @key2)::int
                // OR (SELECT 1 FROM (SELECT pg_advisory_lock(@key)) f)
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

        private static string AddKeyParametersAndGetKeyArguments(DbCommand command, AdvisoryLockKey key)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = "key";
            parameter.Value = key.Key;
            parameter.DbType = DbType.Int64;
            command.Parameters.Add(parameter);
            return "@key";
        }
    }
}
