using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Lock;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Utils
{
    public class LockManager : ILockManager
    {
        private AdvisoryLock? _lock;

        public LockHandle? Acquire(string name, DbContext context, TimeSpan timeout)
        {
            _lock = new AdvisoryLock(new AdvisoryLockKey(name, allowHashing: true), context);

            var @lock = _lock.TryAcquire(timeout).Result;

            return @lock == null ? null : new LockHandle(context);
        }

        public async Task<LockHandle?> TryAcquireAsync(string name, DbContext context,
            TimeSpan timeout = new TimeSpan())
        {
            _lock = new AdvisoryLock(new AdvisoryLockKey(name, allowHashing: true), context);

            var @lock = await _lock.TryAcquire(timeout);

            return @lock == null ? null : new LockHandle(context);
        }
    }
}
