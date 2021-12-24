using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Interfaces;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Common.Utils
{
    public class DirectoryManager : IDirectoryManager
    {
        private string DbDirectoryPath { get; set; } = null!;

        public DirectoryManager()
        {
            SetDbDirectoryPath();
        }

        public void CreateDirectory(string uri)
        {
            // 데이터셋 디렉토리 생성
            DirectoryInfo di = new(Path.Combine(DbDirectoryPath, uri));
            di.Create();

            // 이미지 디렉토리 생성
            di = new(Path.Combine(DbDirectoryPath, uri, "images"));
            di.Create();
        }

        public void DeleteDirectory(string uri)
        {
            DirectoryInfo di = new(Path.Combine(DbDirectoryPath, uri));
            di.Delete();
        }

        // database 경로 설정
        private void SetDbDirectoryPath()
        {
            // 현재 프로젝트 경로
            // CurrentDirectory : ../솔루션 폴더/프로젝트 폴더/
            // 데이터베이스 위치 경로
            DirectoryInfo di = new(Path.Combine(Environment.CurrentDirectory, "database"));

            Console.WriteLine($"DbDirectoryPath:{di}");

            // database 폴더가 없을 시 생성
            if (!di.Exists)
            {
                di.Create();
            }

            DbDirectoryPath = di.ToString();
        }

        public List<string> GetDatasetList()
        {
            DirectoryInfo di = new(DbDirectoryPath);

            var directories = di.GetDirectories();

            List<string> dList = new();
            foreach (var item in directories)
            {
                dList.Add(item.Name);
            }

            return dList;
        }
    }
}
