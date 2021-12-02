using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Migrations.Storage
{
    public partial class DatasetDbMigration_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "label_set",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    class_code_set_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    descriptions = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_label_set", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sample",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    dataset_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    image_count = table.Column<int>(type: "INTEGER", nullable: false),
                    uri = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sample", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "image",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    sample_id = table.Column<string>(type: "TEXT", nullable: false),
                    image_code = table.Column<string>(type: "TEXT", nullable: false),
                    original_filename = table.Column<string>(type: "TEXT", nullable: false),
                    width = table.Column<int>(type: "INTEGER", nullable: false),
                    height = table.Column<int>(type: "INTEGER", nullable: false),
                    channel = table.Column<int>(type: "INTEGER", nullable: false),
                    dtype = table.Column<string>(type: "TEXT", nullable: false),
                    path = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_image_sample_sample_id",
                        column: x => x.sample_id,
                        principalTable: "sample",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "classification_label",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    label_set_id = table.Column<string>(type: "TEXT", nullable: false),
                    sample_id = table.Column<string>(type: "TEXT", nullable: false),
                    image_id = table.Column<string>(type: "TEXT", nullable: false),
                    class_code_id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classification_label", x => x.id);
                    table.ForeignKey(
                        name: "FK_classification_label_image_image_id",
                        column: x => x.image_id,
                        principalTable: "image",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_classification_label_label_set_label_set_id",
                        column: x => x.label_set_id,
                        principalTable: "label_set",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_classification_label_sample_sample_id",
                        column: x => x.sample_id,
                        principalTable: "sample",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "object_detection_label",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    label_set_id = table.Column<string>(type: "TEXT", nullable: false),
                    sample_id = table.Column<string>(type: "TEXT", nullable: false),
                    image_id = table.Column<string>(type: "TEXT", nullable: false),
                    label_path = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_object_detection_label", x => x.id);
                    table.ForeignKey(
                        name: "FK_object_detection_label_image_image_id",
                        column: x => x.image_id,
                        principalTable: "image",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_object_detection_label_label_set_label_set_id",
                        column: x => x.label_set_id,
                        principalTable: "label_set",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_object_detection_label_sample_sample_id",
                        column: x => x.sample_id,
                        principalTable: "sample",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "segmentation_label",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    label_set_id = table.Column<string>(type: "TEXT", nullable: false),
                    sample_id = table.Column<string>(type: "TEXT", nullable: false),
                    image_id = table.Column<string>(type: "TEXT", nullable: false),
                    label_path = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_segmentation_label", x => x.id);
                    table.ForeignKey(
                        name: "FK_segmentation_label_image_image_id",
                        column: x => x.image_id,
                        principalTable: "image",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_segmentation_label_label_set_label_set_id",
                        column: x => x.label_set_id,
                        principalTable: "label_set",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_segmentation_label_sample_sample_id",
                        column: x => x.sample_id,
                        principalTable: "sample",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_classification_label_image_id",
                table: "classification_label",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "IX_classification_label_label_set_id",
                table: "classification_label",
                column: "label_set_id");

            migrationBuilder.CreateIndex(
                name: "IX_classification_label_sample_id",
                table: "classification_label",
                column: "sample_id");

            migrationBuilder.CreateIndex(
                name: "IX_image_sample_id",
                table: "image",
                column: "sample_id");

            migrationBuilder.CreateIndex(
                name: "IX_object_detection_label_image_id",
                table: "object_detection_label",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "IX_object_detection_label_label_set_id",
                table: "object_detection_label",
                column: "label_set_id");

            migrationBuilder.CreateIndex(
                name: "IX_object_detection_label_sample_id",
                table: "object_detection_label",
                column: "sample_id");

            migrationBuilder.CreateIndex(
                name: "IX_segmentation_label_image_id",
                table: "segmentation_label",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "IX_segmentation_label_label_set_id",
                table: "segmentation_label",
                column: "label_set_id");

            migrationBuilder.CreateIndex(
                name: "IX_segmentation_label_sample_id",
                table: "segmentation_label",
                column: "sample_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "classification_label");

            migrationBuilder.DropTable(
                name: "object_detection_label");

            migrationBuilder.DropTable(
                name: "segmentation_label");

            migrationBuilder.DropTable(
                name: "image");

            migrationBuilder.DropTable(
                name: "label_set");

            migrationBuilder.DropTable(
                name: "sample");
        }
    }
}
