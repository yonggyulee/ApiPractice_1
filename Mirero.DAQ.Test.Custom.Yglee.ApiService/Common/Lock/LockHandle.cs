using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Lock
{
    public class LockHandle : IDisposable//, IAsyncDisposable
    {
        private readonly DbContext _context;
        private bool disposedValue;

        public LockHandle(DbContext context)
        {
            _context = context;
        }

        private void _Dispose()
        {
            if (_context.Database.GetDbConnection().State == ConnectionState.Open)
            {
                _context.Database.CloseConnection();
                _context.Database.GetDbConnection().Dispose();
                _context.Dispose();
            }
        }

        //private async ValueTask _DisposeAsync()
        //{
        //    if (_context.Database.GetDbConnection().State == ConnectionState.Open)
        //    {
        //        await _context.Database.CloseConnectionAsync();
        //    }
        //}

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                _Dispose();

                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        ~LockHandle()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
