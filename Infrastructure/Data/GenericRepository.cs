using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        public GenericRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }


        //what we are gonna need to do here in addition to implement these methods is create a new method inside our repository which is gonna allow us apply our specifications 

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            //here where we actually execute the queries and return the data from the database

            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            //here where we actually execute the queries and return the data from the database

            return await ApplySpecification(spec).ToListAsync();
        }
        //what we are gonna do next is how we can actually use what we have done here inside our controller so that we can actually get a list back that uses our specification

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
            //here we have an IQueryable with those expressions that we can then pass to our database
        }
    }
}