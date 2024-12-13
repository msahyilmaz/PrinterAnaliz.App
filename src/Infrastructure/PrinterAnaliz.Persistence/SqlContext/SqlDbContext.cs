using Microsoft.EntityFrameworkCore;
using PrinterAnaliz.Domain.Common;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Persistence.SqlContext
{
    public class SqlDbContext : DbContext
    {
        public string _transactionUser = "Anonymous";
        public SqlDbContext()
        {
        }

        public SqlDbContext(DbContextOptions options) : base(options)
        {
        }
        public SqlDbContext(DbContextOptions options, string transactionUser) : base(options)
        {
            _transactionUser = transactionUser;
        }

        public DbSet<User> User { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Printer> Printer { get; set; }
        public DbSet<PrinterLog> PrinterLog { get; set; }
        public DbSet<UserCustomerRef> UserCustomerRef { get; set; }


   

        public override int SaveChanges()
        {
            OnBeforeSave();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSave();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSave();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSave();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSave()
        {
            var addedEntities = ChangeTracker.Entries()
                                    .Where(i => i.State == EntityState.Added)
                                    .Select(i => (EntityBase)i.Entity);

            var modifiedEntities = ChangeTracker.Entries()
                                    .Where(i => i.State == EntityState.Modified)
                                    .Select(i => (EntityBase)i.Entity);


            PrepareEntities(addedEntities);

            if (modifiedEntities.Count() >= 1)
            {
                UpdateModifiedEntities(modifiedEntities);
            }

        }

        private void PrepareEntities(IEnumerable<EntityBase> entities)
        {

            foreach (var entity in entities)
            {
                entity.CreateFrom = _transactionUser;
                if (entity.CreatedDate == DateTime.MinValue)
                    entity.CreatedDate = DateTime.UtcNow;

            }
        }

        private void UpdateModifiedEntities(IEnumerable<EntityBase> entities)
        {
            foreach (var entity in entities)
            {
                entity.DeletedFrom = _transactionUser;

                if (entity.IsDeleted)
                {
                    if (entity.DeletedDate is null || entity.DeletedDate == DateTime.MinValue)
                        entity.DeletedDate = DateTime.UtcNow;
                }
                else
                {
                    if (entity.UpdatedDate is null || entity.UpdatedDate == DateTime.MinValue)
                        entity.UpdatedDate = DateTime.UtcNow;
                }

            }
        }
       
    }
}
