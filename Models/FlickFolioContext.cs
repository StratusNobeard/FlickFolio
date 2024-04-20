using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

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

    public virtual DbSet<FilmGlumci> FilmGlumci { get; set; }

    public virtual DbSet<FilmZanr> FilmZanrovi { get; set; }

    public virtual DbSet<Glumac> Glumci { get; set; }

    public virtual DbSet<Redatelj> Redatelji { get; set; }

    public virtual DbSet<Serija> Serije { get; set; }

    public virtual DbSet<SerijaGlumac> SerijaGlumci { get; set; }

    public virtual DbSet<SerijaZanr> SerijaZanrovi { get; set; }

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

        modelBuilder.Entity<FilmGlumci>(entity =>
        {
            entity.ToTable("FilmGlumci");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Glumac).WithMany(p => p.FilmGlumcis)
                .HasForeignKey(d => d.GlumacId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.FilmGlumci)
                .HasForeignKey<FilmGlumci>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<FilmZanr>(entity =>
        {
            entity.ToTable("FilmZanr");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.FilmZanr)
                .HasForeignKey<FilmZanr>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Zanr).WithMany(p => p.FilmZanrovi)
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

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Glumac).WithMany(p => p.SerijaGlumacs)
                .HasForeignKey(d => d.GlumacId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.SerijaGlumac)
                .HasForeignKey<SerijaGlumac>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<SerijaZanr>(entity =>
        {
            entity.ToTable("SerijaZanr");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.SerijaZanr)
                .HasForeignKey<SerijaZanr>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Zanr).WithMany(p => p.SerijaZanrovi)
                .HasForeignKey(d => d.ZanrId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Sezona>(entity =>
        {
            entity.ToTable("Sezona");

            entity.HasOne(d => d.Serija).WithMany(p => p.Sezonas)
                .HasForeignKey(d => d.SerijaId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Zanr>(entity =>
        {
            entity.ToTable("Zanr");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
