﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Modules.User.Features.Persistence;

#nullable disable

namespace Modules.User.Features.Persistence.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20241203075138_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("User")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Modules.User.Features.Entities.LabUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Auth0UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LabUsers", "User");
                });

            modelBuilder.Entity("Modules.User.Features.Entities.LinkedAccount", b =>
                {
                    b.Property<string>("Provider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Connection")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsSocial")
                        .HasColumnType("bit");

                    b.Property<Guid>("LabUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Provider", "ProviderKey");

                    b.HasIndex("LabUserId");

                    b.ToTable("LinkedAccount", "User");
                });

            modelBuilder.Entity("Modules.User.Features.Entities.LinkedAccount", b =>
                {
                    b.HasOne("Modules.User.Features.Entities.LabUser", null)
                        .WithMany("LinkedAccounts")
                        .HasForeignKey("LabUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Modules.User.Features.Entities.LabUser", b =>
                {
                    b.Navigation("LinkedAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
