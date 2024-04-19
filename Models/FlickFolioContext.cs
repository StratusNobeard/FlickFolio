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

    public virtual DbSet<Film> Films { get; set; }

    public virtual DbSet<FilmGlumci> FilmGlumcis { get; set; }

    public virtual DbSet<FilmZanr> FilmZanrs { get; set; }

    public virtual DbSet<Glumac> Glumacs { get; set; }

    public virtual DbSet<Redatelj> Redateljs { get; set; }

    public virtual DbSet<Serija> Serijas { get; set; }

    public virtual DbSet<SerijaGlumac> SerijaGlumacs { get; set; }

    public virtual DbSet<SerijaZanr> SerijaZanrs { get; set; }

    public virtual DbSet<Sezona> Sezonas { get; set; }

    public virtual DbSet<Zanr> Zanrs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=.\\Database\\flickFolio.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Film>(entity =>
        {
            entity.ToTable("Film");

            entity.HasOne(d => d.Redatelj).WithMany(p => p.Films)
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

            entity.HasOne(d => d.Zanr).WithMany(p => p.FilmZanrs)
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

            entity.HasOne(d => d.Redatelj).WithMany(p => p.Serijas)
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

            entity.HasOne(d => d.Zanr).WithMany(p => p.SerijaZanrs)
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

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
