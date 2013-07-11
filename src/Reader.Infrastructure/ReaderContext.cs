using System.Data;

namespace Reader.Infrastructure
{
    //[DbModelBuilderVersion(DbModelBuilderVersion.V5_0)]
    public class ReaderContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Role> Role { get; set; }
        public DbSet<Rule> Rule { get; set; }
        public DbSet<Template> Template { get; set; }

        public DbSet<Event> ApprovalEvent { get; set; }
        public DbSet<ContractInfo> ContractInfo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RuleConfiguration());
            modelBuilder.Configurations.Add(new ContractConfiguration());
        }

    }
}
