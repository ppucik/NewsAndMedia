using ASPNETCoreWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text;

namespace ASPNETCoreWebAPI.Repositories;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Specify DbSet properties
    public virtual DbSet<Site> Sites { get; set; } = null!;
    public virtual DbSet<Article> Articles { get; set; } = null!;
    public virtual DbSet<Author> Authors { get; set; } = null!;
    public virtual DbSet<AuthorArticle> AuthorArticles { get; set; } = null!;
    public virtual DbSet<Image> Images { get; set; } = null!;
    public virtual DbSet<AuditLog> AuditLogs { get; set; } = null!;

    // Creating and configuring a Models 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Varianta č.1

        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable(nameof(Article));

            entity.HasKey(a => a.Id);

            entity
                .Property(a => a.Title)
                .HasMaxLength(1000)
                .IsRequired();

            entity
                .HasIndex(a => a.Title);

            entity
                .HasOne(a => a.Site)
                .WithMany(s => s.Articles)
                .HasForeignKey(s => s.SiteId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            entity
                .HasMany(e => e.Authors)
                .WithMany(e => e.Articles)
                .UsingEntity<AuthorArticle>(
                    l => l.HasOne<Author>(e => e.Author)
                        .WithMany(e => e.AuthorArticles)
                        .HasForeignKey(e => e.AuthorId),
                    r => r.HasOne<Article>(e => e.Article)
                        .WithMany(e => e.AuthorArticles)
                        .HasForeignKey(e => e.ArticleId));
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable(nameof(Author));

            entity.HasKey(a => a.Id);

            entity
                .Property(a => a.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity
                .HasIndex(a => a.Name)
                .IsUnique();

            entity
                .HasOne(a => a.Image)
                .WithOne(i => i.Author)
                .HasForeignKey<Image>(i => i.AuthorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        //modelBuilder.Entity<AuthorArticle>(entity =>
        //{
        //    entity.ToTable(nameof(AuthorArticle));

        //    entity.HasKey(aa => new
        //    {
        //        aa.AuthorId,
        //        aa.ArticleId
        //    });

        //    entity
        //        .HasOne(aa => aa.Author)
        //        .WithMany(a => a.AuthorArticles)
        //        .HasForeignKey(aa => aa.AuthorId)
        //        .OnDelete(DeleteBehavior.NoAction);

        //    entity
        //        .HasOne(aa => aa.Article)
        //        .WithMany(a => a.AuthorArticles)
        //        .HasForeignKey(aa => aa.ArticleId)
        //        .OnDelete(DeleteBehavior.NoAction);
        //});

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable(nameof(Image));

            entity.HasKey(i => i.Id);

            entity
                .Property(i => i.Description)
                .IsRequired();
        });

        modelBuilder.Entity<Site>(entity =>
        {
            entity.ToTable(nameof(Site));

            entity.HasKey(s => s.Id);

            entity
                .Property(s => s.CreatedAt)
                .IsRequired();
        });

        // Varianta č.2
        //modelBuilder.ApplyConfiguration(new ArticleConfiguration());

        // Varianta č.3
        //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    // Securing and Tracking Data Change
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var modifiedEntities = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted)
            .ToList();

        foreach (var me in modifiedEntities)
        {
            var auditLog = new AuditLog
            {
                Action = me.State.ToString(),
                TimeStamp = DateTime.UtcNow,
                EntityType = me.Entity.GetType().ToString(),
                Changes = GetEntityChanges(me)
            };

            AuditLogs.Add(auditLog);
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    private static string GetEntityChanges(EntityEntry entry)
    {
        var sb = new StringBuilder();

        foreach (var prop in entry.OriginalValues.Properties)
        {
            var originalValue = entry.OriginalValues[prop];
            var currentValue = entry.CurrentValues[prop];

            if (!Equals(originalValue, currentValue))
            {
                sb.AppendLine($"{prop.Name}: From {originalValue} to {currentValue}");
            }
        }

        return sb.ToString();
    }
}
