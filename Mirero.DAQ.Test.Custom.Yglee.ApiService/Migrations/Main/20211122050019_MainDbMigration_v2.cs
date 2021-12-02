using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Migrations.Main
{
    public partial class MainDbMigration_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dataset_volume_VOLUME_ID",
                table: "dataset");

            migrationBuilder.RenameColumn(
                name: "VOLUME_ID",
                table: "dataset",
                newName: "volume_id");

            migrationBuilder.RenameColumn(
                name: "URI",
                table: "dataset",
                newName: "uri");

            migrationBuilder.RenameColumn(
                name: "NAME",
                table: "dataset",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "DESCRIPTIONS",
                table: "dataset",
                newName: "descriptions");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "dataset",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_dataset_VOLUME_ID",
                table: "dataset",
                newName: "IX_dataset_volume_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_log_job_id",
                table: "job_log",
                column: "job_id");

            migrationBuilder.AddForeignKey(
                name: "FK_dataset_volume_volume_id",
                table: "dataset",
                column: "volume_id",
                principalTable: "volume",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_job_log_job_job_id",
                table: "job_log",
                column: "job_id",
                principalTable: "job",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dataset_volume_volume_id",
                table: "dataset");

            migrationBuilder.DropForeignKey(
                name: "FK_job_log_job_job_id",
                table: "job_log");

            migrationBuilder.DropIndex(
                name: "IX_job_log_job_id",
                table: "job_log");

            migrationBuilder.RenameColumn(
                name: "volume_id",
                table: "dataset",
                newName: "VOLUME_ID");

            migrationBuilder.RenameColumn(
                name: "uri",
                table: "dataset",
                newName: "URI");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "dataset",
                newName: "NAME");

            migrationBuilder.RenameColumn(
                name: "descriptions",
                table: "dataset",
                newName: "DESCRIPTIONS");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "dataset",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_dataset_volume_id",
                table: "dataset",
                newName: "IX_dataset_VOLUME_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_dataset_volume_VOLUME_ID",
                table: "dataset",
                column: "VOLUME_ID",
                principalTable: "volume",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
