using System.Linq;
using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    //entity framework is an object relational mapper and it abstracts the database away from our code and this in turn allows very easily to swap the database that we are using
    //we are abstracting our database away from our code. we don't directly query our database. we use our DbContext methods to query our database 
    public class StoreContext : DbContext
    {
        //this is the constructor that we need so that we can provide it with some options we will give it as the connectionString and we pass that options into the base constructor of DbContext
        //we also will tell the DbContextOptions what type we are passing up here and we will pass StoreContext. we did this because we will be creating an additional Context for sth else later
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        //When we create a migration, OnModelCreating() this is the method that is responsible for creating that migration
        //So we are gonna override this method and tell it to look for our configurations 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }
                }
            }
        }
        
    }
}