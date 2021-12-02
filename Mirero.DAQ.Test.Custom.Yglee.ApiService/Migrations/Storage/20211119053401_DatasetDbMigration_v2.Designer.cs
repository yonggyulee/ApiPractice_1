﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mirero.DAQ.Test.Custom.Yglee.ApiService.Context;

#nullable disable

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Migrations.Storage
{
    [DbContext(typeof(DatasetDbContext))]
    [Migration("20211119053401_DatasetDbMigration_v2")]
    partial class DatasetDbMigration_v2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.ClassificationLabel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<Guid>("ClassCodeID")
                        .HasColumnType("TEXT")
                        .HasColumnName("class_code_id");

                    b.Property<string>("ImageID")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("image_id");

                    b.Property<string>("LabelSetID")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("label_set_id");

                    b.Property<string>("SampleID")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("sample_id");

                    b.HasKey("ID");

                    b.HasIndex("ImageID");

                    b.HasIndex("LabelSetID");

                    b.HasIndex("SampleID");

                    b.ToTable("classification_label");
                });

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.Image", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT")
                        .HasColumnName("id");

                    b.Property<int>("Channel")
                        .HasColumnType("INTEGER")
                        .HasColumnName("channel");

                    b.Property<string>("Dtype")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("dtype");

                    b.Property<int>("Height")
                        .HasColumnType("INTEGER")
                        .HasColumnName("height");

                    b.Property<string>("ImageCode")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("image_code");

                    b.Property<string>("OriginalFilename")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("original_filename");

                    b.Property<string>("SampleID")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("sample_id");

                    b.Property<int>("Width")
                        .HasColumnType("INTEGER")
                        .HasColumnName("width");

                    b.Property<string>("path")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("path");

                    b.HasKey("ID");

                    b.HasIndex("SampleID");

                    b.ToTable("image");
                });

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.LabelSet", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT")
                        .HasColumnName("id");

                    b.Property<Guid>("ClassCodeSetID")
                        .HasColumnType("TEXT")
                        .HasColumnName("class_code_set_id");

                    b.Property<string>("Descriptions")
                        .HasColumnType("TEXT")
                        .HasColumnName("descriptions");

                    b.HasKey("ID");

                    b.ToTable("label_set");
                });

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.ObjectDetectionLabel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("ImageID")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("image_id");

                    b.Property<string>("LabelPath")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("label_path");

                    b.Property<string>("LabelSetID")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("label_set_id");

                    b.Property<string>("SampleID")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("sample_id");

                    b.HasKey("ID");

                    b.HasIndex("ImageID");

                    b.HasIndex("LabelSetID");

                    b.HasIndex("SampleID");

                    b.ToTable("object_detection_label");
                });

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.Sample", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT")
                        .HasColumnName("id");

                    b.Property<Guid>("DatasetID")
                        .HasColumnType("TEXT")
                        .HasColumnName("dataset_id");

                    b.Property<int>("ImageCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0)
                        .HasColumnName("image_count");

                    b.Property<string>("URI")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("uri");

                    b.HasKey("ID");

                    b.ToTable("sample");
                });

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.SegmentationLabel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("ImageID")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("image_id");

                    b.Property<string>("LabelPath")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("label_path");

                    b.Property<string>("LabelSetID")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("label_set_id");

                    b.Property<string>("SampleID")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("sample_id");

                    b.HasKey("ID");

                    b.HasIndex("ImageID");

                    b.HasIndex("LabelSetID");

                    b.HasIndex("SampleID");

                    b.ToTable("segmentation_label");
                });

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.ClassificationLabel", b =>
                {
                    b.HasOne("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.Image", "Image")
                        .WithMany("ClassificationLabels")
                        .HasForeignKey("ImageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.LabelSet", "LabelSet")
                        .WithMany("ClassificationLabels")
                        .HasForeignKey("LabelSetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.Sample", "Sample")
                        .WithMany("ClassificationLabels")
                        .HasForeignKey("SampleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("LabelSet");

                    b.Navigation("Sample");
                });

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.Image", b =>
                {
                    b.HasOne("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.Sample", "Sample")
                        .WithMany("Images")
                        .HasForeignKey("SampleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sample");
                });

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.ObjectDetectionLabel", b =>
                {
                    b.HasOne("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.Image", "Image")
                        .WithMany("ObjectDetectionLabels")
                        .HasForeignKey("ImageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.LabelSet", "LabelSet")
                        .WithMany("ObjectDetectionLabels")
                        .HasForeignKey("LabelSetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.Sample", "Sample")
                        .WithMany("ObjectDetectionLabels")
                        .HasForeignKey("SampleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("LabelSet");

                    b.Navigation("Sample");
                });

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.SegmentationLabel", b =>
                {
                    b.HasOne("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.Image", "Image")
                        .WithMany("SegmentationLabels")
                        .HasForeignKey("ImageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.LabelSet", "LabelSet")
                        .WithMany("SegmentationLabels")
                        .HasForeignKey("LabelSetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.Sample", "Sample")
                        .WithMany("SegmentationLabels")
                        .HasForeignKey("SampleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("LabelSet");

                    b.Navigation("Sample");
                });

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.Image", b =>
                {
                    b.Navigation("ClassificationLabels");

                    b.Navigation("ObjectDetectionLabels");

                    b.Navigation("SegmentationLabels");
                });

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.LabelSet", b =>
                {
                    b.Navigation("ClassificationLabels");

                    b.Navigation("ObjectDetectionLabels");

                    b.Navigation("SegmentationLabels");
                });

            modelBuilder.Entity("Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity.Storage.Sample", b =>
                {
                    b.Navigation("ClassificationLabels");

                    b.Navigation("Images");

                    b.Navigation("ObjectDetectionLabels");

                    b.Navigation("SegmentationLabels");
                });
#pragma warning restore 612, 618
        }
    }
}
