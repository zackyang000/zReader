using System.Data;
using System.Data.Entity;
using Reader.Domain;

namespace Reader.Infrastructure
{
    //[DbModelBuilderVersion(DbModelBuilderVersion.V5_0)]
    public class ReaderContext : DbContext
    {
        public DbSet<Rss> User { get; set; }

        public DbSet<RssFeedFolders> Role { get; set; }
        public DbSet<RssFeedItems> Rule { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

    }
}
