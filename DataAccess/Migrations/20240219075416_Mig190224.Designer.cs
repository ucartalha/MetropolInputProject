﻿// <auto-generated />
using System;
using DataAccess.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(InputContext))]
    [Migration("20240219075416_Mig190224")]
    partial class Mig190224
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Concrete.EmployeeRecord", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FirstRecord")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastRecord")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RemoteEmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Sirket")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("WorkingHour")
                        .HasColumnType("time");

                    b.Property<string>("blok")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("RemoteEmployeeId");

                    b.ToTable("EmployeeRecords");
                });

            modelBuilder.Entity("Entities.Concrete.OverShift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OfficeDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RemoteDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ShiftCount")
                        .HasColumnType("int")
                        .HasColumnName("ShiftCount");

                    b.Property<int>("ShiftDuration")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("ShiftHour")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.ToTable("OverShifts");
                });

            modelBuilder.Entity("Entities.Concrete.Personal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<TimeSpan>("AverageHour")
                        .HasColumnType("time");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RemoteHour")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Personal");
                });

            modelBuilder.Entity("Entities.Concrete.RemoteEmployee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EmployeeDtos");
                });

            modelBuilder.Entity("Entities.Concrete.RemoteWorkEmployee", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RemoteDuration")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("RemoteWorkEmployees");
                });

            modelBuilder.Entity("Entities.Concrete.UploadedFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UploadTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("UploadedFiles");
                });

            modelBuilder.Entity("Entities.Concrete.VpnEmployee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Bytesin")
                        .HasColumnType("int");

                    b.Property<int>("Bytesout")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RemoteEmployeeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("VpnEmployees");
                });

            modelBuilder.Entity("Entities.DTOs.ReaderDataDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeDtoId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeDtoId");

                    b.ToTable("ReaderDataDtos");
                });

            modelBuilder.Entity("Entities.Concrete.EmployeeRecord", b =>
                {
                    b.HasOne("Entities.Concrete.RemoteEmployee", "RemoteEmployee")
                        .WithMany("EmployeeRecords")
                        .HasForeignKey("RemoteEmployeeId");

                    b.Navigation("RemoteEmployee");
                });

            modelBuilder.Entity("Entities.DTOs.ReaderDataDto", b =>
                {
                    b.HasOne("Entities.Concrete.RemoteEmployee", "EmployeeDto")
                        .WithMany("ReaderDataDtos")
                        .HasForeignKey("EmployeeDtoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmployeeDto");
                });

            modelBuilder.Entity("Entities.Concrete.RemoteEmployee", b =>
                {
                    b.Navigation("EmployeeRecords");

                    b.Navigation("ReaderDataDtos");
                });
#pragma warning restore 612, 618
        }
    }
}
