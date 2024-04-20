using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace FlickFolio.Models;

public partial class FlickFolioContext : DbContext
{
    public FlickFolioContext()
    {
    }

    public FlickFolioContext(DbContextOptions<FlickFolioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Film> Filmovi { get; set; }

    public virtual DbSet<FilmGlumac> FilmGlumac { get; set; }

    public virtual DbSet<FilmZanr> FilmZanr { get; set; }

    public virtual DbSet<Glumac> Glumci { get; set; }

    public virtual DbSet<Redatelj> Redatelji { get; set; }

    public virtual DbSet<Serija> Serije { get; set; }

    public virtual DbSet<SerijaGlumac> SerijaGlumac { get; set; }

    public virtual DbSet<SerijaZanr> SerijaZanr { get; set; }

    public virtual DbSet<Sezona> Sezone { get; set; }

    public virtual DbSet<Zanr> Zanrovi { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DatabasePath()}");

    public string DatabasePath()
    {
        var enviroment = Environment.CurrentDirectory;
        string projectDirectory = Directory.GetParent(enviroment).Parent.FullName.Replace("\\bin", ""); ;
        string relativePath = "Database\\flickFolio.db";
        string absolutePath = Path.Combine(projectDirectory, relativePath);
        return absolutePath;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Film>(entity =>
        {
            entity.ToTable("Film");

            entity.HasOne(d => d.Redatelj).WithMany(p => p.Filmovi)
                .HasForeignKey(d => d.RedateljId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<FilmGlumac>(entity =>
        {
            entity.ToTable("FilmGlumci");

            entity.HasOne(d => d.Film).WithMany(p => p.FilmGlumac)
                .HasForeignKey(d => d.FilmId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Glumac).WithMany(p => p.FilmGlumac)
                .HasForeignKey(d => d.GlumacId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<FilmZanr>(entity =>
        {
            entity.ToTable("FilmZanr");

            entity.HasOne(d => d.Film).WithMany(p => p.FilmZanr)
                .HasForeignKey(d => d.FilmId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Zanr).WithMany(p => p.FilmZanr)
                .HasForeignKey(d => d.ZanrId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Glumac>(entity =>
        {
            entity.ToTable("Glumac");
        });

        modelBuilder.Entity<Redatelj>(entity =>
        {
            entity.ToTable("Redatelj");
        });

        modelBuilder.Entity<Serija>(entity =>
        {
            entity.ToTable("Serija");

            entity.HasOne(d => d.Redatelj).WithMany(p => p.Serije)
                .HasForeignKey(d => d.RedateljId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<SerijaGlumac>(entity =>
        {
            entity.ToTable("SerijaGlumac");

            entity.HasOne(d => d.Glumac).WithMany(p => p.SerijaGlumac)
                .HasForeignKey(d => d.GlumacId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Serija).WithMany(p => p.SerijaGlumac)
                .HasForeignKey(d => d.SerijaId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<SerijaZanr>(entity =>
        {
            entity.ToTable("SerijaZanr");

            entity.HasOne(d => d.Serija).WithMany(p => p.SerijaZanr)
                .HasForeignKey(d => d.SerijaId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Zanr).WithMany(p => p.SerijaZanr)
                .HasForeignKey(d => d.ZanrId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Sezona>(entity =>
        {
            entity.ToTable("Sezona");

            entity.HasOne(d => d.Serija).WithMany(p => p.Sezone)
                .HasForeignKey(d => d.SerijaId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Zanr>(entity =>
        {
            entity.ToTable("Zanr");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
