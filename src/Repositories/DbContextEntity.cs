using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Entities;

namespace DbEntity
{
    public class DbContextEntity : DbContext
    {
        public IDbContextTransaction _transaction;
        public DbContextEntity(DbContextOptions<DbContextEntity> options)
        : base(options) { }

        #region 

        public DbSet<Metadata> Metadatas { get; set; }

        #endregion

        #region Order

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderPay> OrderPays { get; set; }

        public DbSet<OrderInventory> OrderInventories { get; set; }

        #endregion

        #region Product

        public DbSet<Product> Products { get; set; }

        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductSpecification> ProductSpecifications { get; set; }

        public DbSet<InventorySpecification> InventorySpecifications { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Specification> Specifications { get; set; }

        public DbSet<SpecificationContent> SpecificationContents { get; set; }

        #endregion

        #region user

        public DbSet<User> Users { get; set; }

        public DbSet<UserDetail> UserDetails { get; set; }

        #endregion

        public async void BeginTransaction()
        {
            _transaction = await Database.BeginTransactionAsync();
        }

        public async void Commit()
        {
            try
            {
                await SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async void Rollback()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}