using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Lock;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Utils
{
    public class LockManager : ILockManager
    {
        private AdvisoryLock _lock;

        public async Task AcquireAsync()
        {

        }

        public async Task TryAcquireAsync()
        {

        }

        public void SetLock(string name)
        {
            _lock = new AdvisoryLock(new AdvisoryLockKey(name, allowHashing: true));
        }
    }
}
