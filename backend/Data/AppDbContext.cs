using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Site> Sites => Set<Site>();
    public DbSet<ClientSiteAccess> ClientSiteAccesses => Set<ClientSiteAccess>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<ContentBlock> ContentBlocks => Set<ContentBlock>();
    public DbSet<Media> Media => Set<Media>();
    public DbSet<MediaVariant> MediaVariants => Set<MediaVariant>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Role).HasMaxLength(20);
            e.Property(u => u.Email).HasMaxLength(256);
            e.Property(u => u.Name).HasMaxLength(200);

            e.HasOne(u => u.CreatedBy)
                .WithMany()
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Site
        modelBuilder.Entity<Site>(e =>
        {
            e.HasIndex(s => s.Slug).IsUnique();
            e.HasIndex(s => s.ApiKey).IsUnique();
            e.Property(s => s.Name).HasMaxLength(200);
            e.Property(s => s.Slug).HasMaxLength(100);
            e.Property(s => s.ApiKey).HasMaxLength(100);
            e.Property(s => s.Domain).HasMaxLength(500);

            e.HasOne(s => s.Developer)
                .WithMany(u => u.Sites)
                .HasForeignKey(s => s.DeveloperId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ClientSiteAccess - composite key
        modelBuilder.Entity<ClientSiteAccess>(e =>
        {
            e.HasKey(c => new { c.ClientId, c.SiteId });

            e.HasOne(c => c.Client)
                .WithMany(u => u.ClientSiteAccesses)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(c => c.Site)
                .WithMany(s => s.ClientSiteAccesses)
                .HasForeignKey(c => c.SiteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Section
        modelBuilder.Entity<Section>(e =>
        {
            e.HasIndex(s => new { s.SiteId, s.Slug }).IsUnique();
            e.Property(s => s.Name).HasMaxLength(200);
            e.Property(s => s.Slug).HasMaxLength(100);

            e.HasOne(s => s.Site)
                .WithMany(s => s.Sections)
                .HasForeignKey(s => s.SiteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ContentBlock
        modelBuilder.Entity<ContentBlock>(e =>
        {
            e.HasOne(c => c.Section)
                .WithMany(s => s.ContentBlocks)
                .HasForeignKey(c => c.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Media
        modelBuilder.Entity<Media>(e =>
        {
            e.Property(m => m.FileName).HasMaxLength(500);
            e.Property(m => m.ContentType).HasMaxLength(100);

            e.HasOne(m => m.UploadedBy)
                .WithMany()
                .HasForeignKey(m => m.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(m => m.Site)
                .WithMany(s => s.MediaFiles)
                .HasForeignKey(m => m.SiteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // MediaVariant
        modelBuilder.Entity<MediaVariant>(e =>
        {
            e.Property(v => v.VariantType).HasMaxLength(20);
            e.Property(v => v.Format).HasMaxLength(20);

            e.HasOne(v => v.Media)
                .WithMany(m => m.Variants)
                .HasForeignKey(v => v.MediaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // RefreshToken
        modelBuilder.Entity<RefreshToken>(e =>
        {
            e.HasIndex(r => r.Token).IsUnique();

            e.HasOne(r => r.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
