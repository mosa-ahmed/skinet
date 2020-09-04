using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

//here we check to see is the criteria is not equal to null, it won't be because we know we 've the id
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria); //e.g: query.Where(p => p.productTypeId == id); 
            }

            //our Includes is gonna be Aggregate because we are combining all of our Include() operations because we may have more than one Include() and we want to accumulate these into an expression
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}