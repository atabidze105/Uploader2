using System;
using System.Collections.Generic;
using DataUploaderApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DataUploaderApp.Context;

public partial class DefaultDbContext : DbContext
{
    public DefaultDbContext()
    {
    }

    public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Cathegory> Cathegories { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=91.186.197.80:5432;Database=default_db;Username=gen_user;password=6?,65\\fIp7(lqq");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("article_pk");

            entity.ToTable("article", "uploadstat");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AddDate).HasColumnName("add_date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdCathegorie).HasColumnName("id_cathegorie");
            entity.Property(e => e.Music).HasColumnName("music");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Photo)
                .HasColumnType("character varying")
                .HasColumnName("photo");
            entity.Property(e => e.Video).HasColumnName("video");

            entity.HasOne(d => d.IdCathegorieNavigation).WithMany(p => p.Articles)
                .HasForeignKey(d => d.IdCathegorie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("article_cathegory_fk");

            entity.HasMany(d => d.IdPhotos).WithMany(p => p.IdArticls)
                .UsingEntity<Dictionary<string, object>>(
                    "ListPhoto",
                    r => r.HasOne<Photo>().WithMany()
                        .HasForeignKey("IdPhoto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("list_photo_photo_fk"),
                    l => l.HasOne<Article>().WithMany()
                        .HasForeignKey("IdArticl")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("list_photo_article_fk"),
                    j =>
                    {
                        j.HasKey("IdArticl", "IdPhoto").HasName("list_photo_pk");
                        j.ToTable("list_photo", "uploadstat");
                        j.IndexerProperty<int>("IdArticl").HasColumnName("id_articl");
                        j.IndexerProperty<int>("IdPhoto").HasColumnName("id_photo");
                    });
        });

        modelBuilder.Entity<Cathegory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cathegory_pk");

            entity.ToTable("cathegory", "uploadstat");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("photo_pk");

            entity.ToTable("photo", "uploadstat");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Photo1).HasColumnName("photo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
