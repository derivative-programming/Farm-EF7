using Microsoft.EntityFrameworkCore;
using FS.Farm.EF.Models;
using Microsoft.Data.SqlClient;

namespace FS.Farm.EF
{
    public class FarmDbContext : DbContext
    {
        public FarmDbContext(DbContextOptions<FarmDbContext> options) : base(options)
        {
        } 

        public DbSet<Customer> CustomerSet { get; set; }
        public DbSet<CustomerRole> CustomerRoleSet { get; set; }
        public DbSet<DateGreaterThanFilter> DateGreaterThanFilterSet { get; set; }
        public DbSet<DFMaintenance> DFMaintenanceSet { get; set; }
        public DbSet<DFTDependency> DFTDependencySet { get; set; }
        public DbSet<DynaFlow> DynaFlowSet { get; set; }
        public DbSet<DynaFlowTask> DynaFlowTaskSet { get; set; }
        public DbSet<DynaFlowTaskType> DynaFlowTaskTypeSet { get; set; }
        public DbSet<DynaFlowType> DynaFlowTypeSet { get; set; }
        public DbSet<DynaFlowTypeSchedule> DynaFlowTypeScheduleSet { get; set; } 
        public DbSet<ErrorLog> ErrorLogSet { get; set; }
        public DbSet<Flavor> FlavorSet { get; set; }
        public DbSet<Land> LandSet { get; set; }
        public DbSet<Organization> OrganizationSet { get; set; }
        public DbSet<OrgApiKey> OrgApiKeySet { get; set; }
        public DbSet<OrgCustomer> OrgCustomerSet { get; set; }
        public DbSet<Pac> PacSet { get; set; }
        public DbSet<Plant> PlantSet { get; set; }
        public DbSet<Role> RoleSet { get; set; }
        public DbSet<Tac> TacSet { get; set; }
        public DbSet<TriStateFilter> TriStateFilterSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FarmDbContext).Assembly); 
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("insert_utc_date_time").CurrentValue = DateTime.UtcNow;
                    entry.Property("last_updated_utc_date_time").CurrentValue = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("last_updated_utc_date_time").CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}