using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Fingo.Auth.DbAccess.Context;

namespace Fingo.Auth.DbAccess.Migrations
{
    [DbContext(typeof(AuthServerContext))]
    [Migration("20160810121738_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Fingo.Auth.DbAccess.Models.ClientInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContactData")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<int>("ProjectIdFk");

                    b.HasKey("Id");

                    b.HasIndex("ProjectIdFk")
                        .IsUnique();

                    b.ToTable("ClientInformation");
                });

            modelBuilder.Entity("Fingo.Auth.DbAccess.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 30);

                    b.Property<Guid>("ProjectGuid");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Fingo.Auth.DbAccess.Models.ProjectUser", b =>
                {
                    b.Property<int>("ProjectId");

                    b.Property<int>("UserId");

                    b.HasKey("ProjectId", "UserId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("ProjectUser");
                });

            modelBuilder.Entity("Fingo.Auth.DbAccess.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivationToken");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("FirstName")
                        .HasAnnotation("MaxLength", 30);

                    b.Property<string>("LastName")
                        .HasAnnotation("MaxLength", 30);

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<DateTime>("ModificationDate");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Fingo.Auth.DbAccess.Models.ClientInformation", b =>
                {
                    b.HasOne("Fingo.Auth.DbAccess.Models.Project", "Project")
                        .WithOne("Information")
                        .HasForeignKey("Fingo.Auth.DbAccess.Models.ClientInformation", "ProjectIdFk")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fingo.Auth.DbAccess.Models.ProjectUser", b =>
                {
                    b.HasOne("Fingo.Auth.DbAccess.Models.Project", "Project")
                        .WithMany("ProjectUsers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fingo.Auth.DbAccess.Models.User", "User")
                        .WithMany("ProjectUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
