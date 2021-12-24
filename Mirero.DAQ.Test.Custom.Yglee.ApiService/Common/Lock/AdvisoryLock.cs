using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Lock
{
    public class AdvisoryLock
    {
        private readonly AdvisoryLockKey _key;

        public AdvisoryLock(AdvisoryLockKey key)
        {
            _key = key;
        }

        public async ValueTask<object?> TryAcquire(DbContext context, TimeSpan timeout)
        {
            await using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "select * from dataset;";
                cmd.CommandType = CommandType.Text;

                await context.Database.OpenConnectionAsync();
            }

            return 0;
        }
    }
}
