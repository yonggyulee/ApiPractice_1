using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Migrations.Main
{
    public partial class MainDbMigration_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "auth",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    descriptions = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "batch_job",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: false),
                    total_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    end_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ready_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    progress_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_date"),
                    progress_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_date")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batch_job", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "class_code_set",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    task_type = table.Column<string>(type: "text", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_code_set", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "job_log",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_id = table.Column<int>(type: "integer", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    message = table.Column<string>(type: "text", nullable: false),
                    exception = table.Column<string>(type: "text", nullable: true),
                    properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "server",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    ip = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<int>(type: "integer", nullable: false),
                    server_type = table.Column<string>(type: "text", nullable: false),
                    os_type = table.Column<string>(type: "text", nullable: false),
                    os_version = table.Column<string>(type: "text", nullable: false),
                    cpu_count = table.Column<int>(type: "integer", nullable: false),
                    cpu_memory = table.Column<int>(type: "integer", nullable: false),
                    gpu_name = table.Column<string>(type: "text", nullable: true),
                    gpu_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    gpu_memory = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "volume",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    uri = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    usage = table.Column<int>(type: "integer", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_volume", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "job",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    batch_job_id = table.Column<int>(type: "integer", nullable: false),
                    worker_id = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    ready_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    progress_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_date"),
                    progress_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_date")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_batch_job_batch_job_id",
                        column: x => x.batch_job_id,
                        principalTable: "batch_job",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "class_code",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    class_code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    info = table.Column<string>(type: "text", nullable: false),
                    class_code_set_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_code", x => x.id);
                    table.ForeignKey(
                        name: "FK_class_code_class_code_set_class_code_set_id",
                        column: x => x.class_code_set_id,
                        principalTable: "class_code_set",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "worker",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    server_id = table.Column<string>(type: "text", nullable: false),
                    worker_type = table.Column<string>(type: "text", nullable: false),
                    properties = table.Column<string>(type: "text", nullable: true),
                    cpu_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    cpu_memory = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    gpu_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    gpu_memory = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_worker", x => x.id);
                    table.ForeignKey(
                        name: "FK_worker_server_server_id",
                        column: x => x.server_id,
                        principalTable: "server",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_auth_map",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    auth_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_auth_map", x => new { x.user_id, x.auth_id });
                    table.ForeignKey(
                        name: "FK_user_auth_map_auth_auth_id",
                        column: x => x.auth_id,
                        principalTable: "auth",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_auth_map_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dataset",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    VOLUME_ID = table.Column<string>(type: "text", nullable: false),
                    URI = table.Column<string>(type: "text", nullable: false),
                    DESCRIPTIONS = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dataset", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dataset_volume_VOLUME_ID",
                        column: x => x.VOLUME_ID,
                        principalTable: "volume",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "artifact",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    volume_id = table.Column<string>(type: "text", nullable: false),
                    uri = table.Column<string>(type: "text", nullable: false),
                    descriptions = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artifact", x => x.id);
                    table.ForeignKey(
                        name: "FK_artifact_job_job_id",
                        column: x => x.job_id,
                        principalTable: "job",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_artifact_volume_volume_id",
                        column: x => x.volume_id,
                        principalTable: "volume",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_artifact_job_id",
                table: "artifact",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_artifact_volume_id",
                table: "artifact",
                column: "volume_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_code_class_code_set_id",
                table: "class_code",
                column: "class_code_set_id");

            migrationBuilder.CreateIndex(
                name: "IX_dataset_VOLUME_ID",
                table: "dataset",
                column: "VOLUME_ID");

            migrationBuilder.CreateIndex(
                name: "IX_job_batch_job_id",
                table: "job",
                column: "batch_job_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_auth_map_auth_id",
                table: "user_auth_map",
                column: "auth_id");

            migrationBuilder.CreateIndex(
                name: "IX_worker_server_id",
                table: "worker",
                column: "server_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "artifact");

            migrationBuilder.DropTable(
                name: "class_code");

            migrationBuilder.DropTable(
                name: "dataset");

            migrationBuilder.DropTable(
                name: "job_log");

            migrationBuilder.DropTable(
                name: "user_auth_map");

            migrationBuilder.DropTable(
                name: "worker");

            migrationBuilder.DropTable(
                name: "job");

            migrationBuilder.DropTable(
                name: "class_code_set");

            migrationBuilder.DropTable(
                name: "volume");

            migrationBuilder.DropTable(
                name: "auth");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "server");

            migrationBuilder.DropTable(
                name: "batch_job");
        }
    }
}
