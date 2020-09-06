using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            //FindFisrt() => doesn't accept an IQueryable so we used FirstOrDefault() OR SingleOrDefault()
            //The only difference between them is that FirstOrDefault() returns an entity as soon as it finds it in the list and it won't throw an exception if there is more than one in the list
            //whereas SingleOrDefault() if it finds more than one of the same entity in the list, it's gonna throw an exception. But they both do the same job for us
            return await _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            //An IQueryable isn't sth that is executed against the database, it's building up a list of expressions to query the database against
            
            //Where(), Include(), ... all these expressions that return IQueryable Doesn't execute against the database until we execute .ToListAsync() or .ToList()
            //.ToList() command is when the query goes to the database and executes whatever in these expressions of Where(), Include() ......
            //so we pass an IQueryable to the ToList() to tell it what we want to fetch from the database

            //we also added support for Ordering (Sorting) into our specification so that we can make use of them and send this kind of query to our Specification Evaluator and then make use of that in our Generic Repository 
            return await _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductsBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductsTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}