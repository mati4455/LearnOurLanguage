using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Model.Models;

namespace Model.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Model.Models.Database.Dictionary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<int>("FirstLanguageId");

                    b.Property<bool>("IsPublic");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<int?>("ParentDictionaryId");

                    b.Property<int>("SecondLanguageId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("FirstLanguageId");

                    b.HasIndex("SecondLanguageId");

                    b.HasIndex("UserId");

                    b.ToTable("Dictionaries");
                });

            modelBuilder.Entity("Model.Models.Database.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Model.Models.Database.GameSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateEnd");

                    b.Property<DateTime>("DateStart");

                    b.Property<int>("DictionaryId");

                    b.Property<int>("GameId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("DictionaryId");

                    b.HasIndex("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("GameSessions");
                });

            modelBuilder.Entity("Model.Models.Database.GameSessionTranslation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Correct");

                    b.Property<decimal>("Duration");

                    b.Property<int>("GameSessionId");

                    b.Property<int>("TranslationId");

                    b.HasKey("Id");

                    b.HasIndex("GameSessionId");

                    b.HasIndex("TranslationId");

                    b.ToTable("GameSessionTranslations");
                });

            modelBuilder.Entity("Model.Models.Database.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Model.Models.Database.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessLevel");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Model.Models.Database.Translation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DictionaryId");

                    b.Property<string>("FirstLangWord")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<string>("SecondLangWord")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.HasKey("Id");

                    b.HasIndex("DictionaryId");

                    b.ToTable("Translations");
                });

            modelBuilder.Entity("Model.Models.Database.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<int>("RoleId");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("varbinary(16)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Model.Models.Database.Dictionary", b =>
                {
                    b.HasOne("Model.Models.Database.Language", "FirstLanguage")
                        .WithMany()
                        .HasForeignKey("FirstLanguageId");

                    b.HasOne("Model.Models.Database.Language", "SecondLanguage")
                        .WithMany()
                        .HasForeignKey("SecondLanguageId");

                    b.HasOne("Model.Models.Database.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Model.Models.Database.GameSession", b =>
                {
                    b.HasOne("Model.Models.Database.Dictionary", "Dictionary")
                        .WithMany()
                        .HasForeignKey("DictionaryId");

                    b.HasOne("Model.Models.Database.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("Model.Models.Database.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Model.Models.Database.GameSessionTranslation", b =>
                {
                    b.HasOne("Model.Models.Database.GameSession", "GameSession")
                        .WithMany()
                        .HasForeignKey("GameSessionId");

                    b.HasOne("Model.Models.Database.Translation", "Translation")
                        .WithMany()
                        .HasForeignKey("TranslationId");
                });

            modelBuilder.Entity("Model.Models.Database.Translation", b =>
                {
                    b.HasOne("Model.Models.Database.Dictionary", "Dictionary")
                        .WithMany()
                        .HasForeignKey("DictionaryId");
                });

            modelBuilder.Entity("Model.Models.Database.User", b =>
                {
                    b.HasOne("Model.Models.Database.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });
        }
    }
}
