using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Lock;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces
{
    public interface ILockManager
    {
        public LockHandle? Acquire(string name, DbContext context, TimeSpan timeout);
        public Task<LockHandle?> TryAcquireAsync(string name, DbContext context, TimeSpan timeout = new TimeSpan());
    }
}
