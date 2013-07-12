using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using Reader.Domain;

namespace Reader.Infrastructure
{
    //[DbModelBuilderVersion(DbModelBuilderVersion.V5_0)]
    public class ReaderContext : DbContext
    {
        public DbSet<Rss> Rss { get; set; }
        public DbSet<RssFeedFolders> RssFeedFolders { get; set; }
        public DbSet<RssFeedItems> RssFeedItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rss>().ToTable("Rss");
            modelBuilder.Entity<RssFeedFolders>().ToTable("RssFeedFolders");
            modelBuilder.Entity<RssFeedItems>().ToTable("RssFeedItems");

              modelBuilder.Entity<Rss>().Property(p=>p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
              modelBuilder.Entity<RssFeedFolders>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
              modelBuilder.Entity<RssFeedItems>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        } 
    }
}
