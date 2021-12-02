using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Context
{
    public class DatasetDbContext : DbContext
    {
        private string DbFileName = "dataset.db";
        private string? ConnectionString { get; set; }

        public DbSet<Sample> Samples { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<LabelSet> LabelSets { get; set; } = null!;
        public DbSet<ClassificationLabel> ClassificationLabels { get; set; } = null!;
        public DbSet<ObjectDetectionLabel> ObjectDetectionLabels { get; set; } = null!;
        public DbSet<SegmentationLabel> SegmentationLabels { get; set; } = null!;
        
        public DatasetDbContext(string dsId = "dataset_1")
        {
            SetConnectionString(dsId);
        }

        public static DatasetDbContext GetInstance(string dsId)
        {
            return new DatasetDbContext(dsId);
        }

        public async Task<string> MigrateDb(string version = "DatasetDbMigration_v2")
        {
            Console.WriteLine($"Migration Version: {version}");
            // 기존에 Migration한 데이터베이스 정보를 db 파일에 적용.
            // id 위치에 db 파일이 없을 시 migrate하여 db 파일 생성.
            var migrator = this.GetInfrastructure().GetService<IMigrator>();
            if (migrator != null) await migrator.MigrateAsync(version);
            // 가장 최근에 적용된 Migration을 반환.
            var lastAppliedMigration = (await this.Database.GetAppliedMigrationsAsync()).Last();
            Console.WriteLine($"You're on schema version: {lastAppliedMigration}");
            
            return lastAppliedMigration;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (ConnectionString != null) options.UseSqlite(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sample>().HasMany(s => s.Images).WithOne(i => i.Sample);
            
            // id 자동 증가
            modelBuilder.Entity<ClassificationLabel>()
                .Property(c => c.ID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<ObjectDetectionLabel>()
                .Property(o => o.ID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<SegmentationLabel>()
                .Property(s => s.ID)
                .ValueGeneratedOnAdd();

            // nullabel 설정
            modelBuilder.Entity<LabelSet>()
                .Property(l => l.Descriptions)
                .IsRequired(false);
            
            // 기본값 설정
            modelBuilder.Entity<Sample>()
                .Property(s => s.ImageCount)
                .HasDefaultValue(0);
            
            // guid 자동 생성 설정
            // modelBuilder.Entity<ClassificationLabel>()
            //     .Property(c => c.ID)
            //     .HasDefaultValueSql("uuid()"); 
            // modelBuilder.Entity<ObjectDetectionLabel>()
            //     .Property(o => o.ID)
            //     .HasDefaultValueSql("uuid()");
            // modelBuilder.Entity<SegmentationLabel>()
            //     .Property(s => s.ID)
            //     .HasDefaultValueSql("uuid()");
        }
        
        // 데이터베이스 위치 경로 반환.
        // 반환값 : ../솔루션 폴더/프로젝트 폴더/database
        private void SetConnectionString(string datasetId)
        {
            // 현재 프로젝트 경로
            // CurrentDirectory : ../솔루션 폴더/프로젝트 폴더
            // 데이터베이스 위치 경로
            DirectoryInfo databaseDi = new DirectoryInfo(
                    Path.Combine(Environment.CurrentDirectory, "database", datasetId)
                    );

            // database 폴더가 없을 시 database 폴더 생성
            if (!databaseDi.Exists)
            {
                databaseDi.Create();
            }

            // database_path : ../솔루션 폴더/프로젝트 폴더/database/{dataset_id}/dataset.db
            string databasePath = Path.Combine(databaseDi.ToString(), DbFileName);
            Console.WriteLine($"SqliteContext.GetDbPath Return : {databasePath}");

            ConnectionString = $"Data Source={databasePath}";
        }
    }
}