using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Main;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Context
{
    // TODO PostgreSqlMainDbContext 과 같이 명시적인 이름이 필요할 수도 있습니다 
    // TODO DbSet<T> 를 담는 BaseMainDbContext 를 만들면...
    //       - PostgreSqlMainDbContext 
    //       - SqliteMainDbContext 
    //      와 같은 구체적 Backend DB 에 대한 설정과 Model Build 를 개별로 관리할 수 있지 않을까요 
    public class MainDbContext : DbContext
    {
        public DbSet<Server> Servers { get; set; } = null!;
        public DbSet<Worker> Workers { get; set; } = null!;
        public DbSet<Volume> Volumes { get; set; } = null!;
        public DbSet<Dataset> Datasets { get; set; } = null!; //@CHECKME
        public DbSet<ClassCodeSet> ClassCodeSets { get; set; } = null!;
        public DbSet<ClassCode> ClassCodes { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Auth> Auths { get; set; } = null!;
        public DbSet<UserAuthMap> UserAuthMaps { get; set; } = null!;
        public DbSet<BatchJob> BatchJobs { get; set; } = null!;
        public DbSet<Job> Jobs { get; set; } = null!;
        public DbSet<Artifact> Artifacts { get; set; } = null!;
        public DbSet<JobLog> JobLogs { get; set; } = null!;
        
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        {
        }
        
        public async Task<string> MigrateDb(string version = "MainDbMigration_v1")
        {
            Console.WriteLine($"version = {version}");
            // 기존에 Migration한 데이터베이스 정보를 db 파일에 적용.
            // id 위치에 db 파일이 없을 시 migrate하여 db 파일 생성.
            var migrator = this.GetInfrastructure().GetService<IMigrator>();
            if (migrator != null) await migrator.MigrateAsync(version);
            // 가장 최근에 적용된 Migration을 반환.
            var lastAppliedMigration = (await this.Database.GetAppliedMigrationsAsync()).Last();
            Console.WriteLine($"You're on schema version: {lastAppliedMigration}");

            return lastAppliedMigration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.HasPostgresExtension("uuid-ossp");     // Postgresql의 guid 자동 생성 Extension
            // UserAuthMap 테이블 복합 키 설정
            modelBuilder.Entity<UserAuthMap>().HasKey(table => new {table.UserId, table.AuthId});

            // nullable 설정
            modelBuilder.Entity<Server>()
                .Property(s => s.GpuName)
                .IsRequired(false);
            modelBuilder.Entity<Artifact>()
                .Property(a => a.Descriptions)
                .IsRequired(false);
            modelBuilder.Entity<Dataset>()
                .Property(d => d.Descriptions)
                .IsRequired(false);
            modelBuilder.Entity<JobLog>()
                .Property(j => j.Exception)
                .IsRequired(false);
            modelBuilder.Entity<User>()
                .Property(u => u.Descriptions)
                .IsRequired(false);
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired(false);
            modelBuilder.Entity<Worker>()
                .Property(w => w.Properties)
                .IsRequired(false);
            modelBuilder.Entity<Auth>()
                .Property(a => a.Descriptions)
                .IsRequired(false);

            // 기본값 설정
            modelBuilder.Entity<Server>()
                .Property(s => s.GpuCount)
                .HasDefaultValue(0);
            modelBuilder.Entity<Server>()
                .Property(s => s.GpuMemory)
                .HasDefaultValue(0);
            modelBuilder.Entity<Worker>()
                .Property(w => w.GpuCount)
                .HasDefaultValue(0);
            modelBuilder.Entity<Worker>()
                .Property(w => w.GpuMemory)
                .HasDefaultValue(0);
            modelBuilder.Entity<Worker>()
                .Property(w => w.CpuCount)
                .HasDefaultValue(0);
            modelBuilder.Entity<Worker>()
                .Property(w => w.CpuMemory)
                .HasDefaultValue(0);
            modelBuilder.Entity<BatchJob>()
                .Property(b => b.TotalCount)
                .HasDefaultValue(1);
            modelBuilder.Entity<BatchJob>()
                .Property(b => b.EndCount)
                .HasDefaultValue(0);
            
            // 현재 시간 입력 함수 설정
            modelBuilder.Entity<ClassCodeSet>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("now()");    // Postgresql 함수
            modelBuilder.Entity<ClassCodeSet>()
                .Property(c => c.UpdatedAt)
                .HasDefaultValueSql("now()");    // Postgresql 함수
            modelBuilder.Entity<JobLog>()
                .Property(j => j.Time)
                .HasDefaultValueSql("now()");
            modelBuilder.Entity<BatchJob>()
                .Property(b => b.ReadyStart)
                .HasDefaultValueSql("now()");
            modelBuilder.Entity<BatchJob>()
                .Property(b => b.ProgressStart)
                .HasDefaultValueSql("current_date");
            modelBuilder.Entity<BatchJob>()
                .Property(b => b.ProgressEnd)
                .HasDefaultValueSql("current_date");
            modelBuilder.Entity<Job>()
                .Property(j => j.ReadyStart)
                .HasDefaultValueSql("now()");
            modelBuilder.Entity<Job>()
                .Property(j => j.ProgressStart)
                .HasDefaultValueSql("current_date");
            modelBuilder.Entity<Job>()
                .Property(j => j.ProgressEnd)
                .HasDefaultValueSql("current_date");

            // 자동 증가 설정
            // Postgresql DB 사용
            modelBuilder.Entity<ClassCode>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();  
            modelBuilder.Entity<Artifact>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd(); 
            modelBuilder.Entity<BatchJob>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd(); 
            modelBuilder.Entity<Dataset>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd(); 
            modelBuilder.Entity<Job>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<JobLog>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd(); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO 아래 코드는 EF Core로 하여금 Table 및 Column 명칭을 C# Keyword 로 부터 유추할 경우, Snake Casing을 강제합니다. Postgresql의 경우 이렇게 하면 어떤점이 좋을까요
            // (ref: https://www.npgsql.org/efcore/modeling/table-column-naming.html)
            // optionsBuilder.UseSnakeCaseNamingConvention();
        }
    }
}