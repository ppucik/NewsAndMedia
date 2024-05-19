namespace ASPNETCoreWebAPI.Repositories.Configurations;

// Varianta č.2 - alternativa konfiguracie

//public class ArticleConfiguration : IEntityTypeConfiguration<Article>
//{
//    public void Configure(EntityTypeBuilder<Article> builder)
//    {
//        builder.ToTable(nameof(Article));
//        builder.HasKey(a => a.Id);
//        builder.Property(a => a.Title).HasMaxLength(1000).IsRequired();
//        builder.HasIndex(a => a.Title);

//        // atd...
//    }
//}
