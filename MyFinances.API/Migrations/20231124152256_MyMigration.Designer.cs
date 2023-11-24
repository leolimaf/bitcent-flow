﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyFinances.API.Data;

#nullable disable

namespace MyFinances.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231124152256_MyMigration")]
    partial class MyMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MyFinances.Domain.Models.TransacaoFinanceira", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2")
                        .HasColumnName("DATA");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("DESCRICAO");

                    b.Property<Guid>("IdUsuario")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID_USUARIO");

                    b.Property<int>("Tipo")
                        .HasColumnType("int")
                        .HasColumnName("TIPO");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("VALOR");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuario");

                    b.ToTable("TRANSACAO_FINANCEIRA");
                });

            modelBuilder.Entity("MyFinances.Domain.Models.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("EMAIL");

                    b.Property<bool>("IsAdministrador")
                        .HasColumnType("bit")
                        .HasColumnName("ADMINISTRADOR");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("NOME");

                    b.Property<string>("SenhaHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("SENHA_HASH");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TOKEN");

                    b.Property<DateTime?>("ValidadeToken")
                        .HasColumnType("datetime2")
                        .HasColumnName("VALIDADE_TOKEN");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Nome")
                        .IsUnique();

                    b.ToTable("USUARIO");
                });

            modelBuilder.Entity("MyFinances.Domain.Models.TransacaoFinanceira", b =>
                {
                    b.HasOne("MyFinances.Domain.Models.Usuario", "Usuario")
                        .WithMany("TransacaoFinanceiras")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MyFinances.Domain.Models.Usuario", b =>
                {
                    b.Navigation("TransacaoFinanceiras");
                });
#pragma warning restore 612, 618
        }
    }
}