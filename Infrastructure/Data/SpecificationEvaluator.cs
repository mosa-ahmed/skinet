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

            //The order of these if() statements is important. So the paging operators need to come after any filtering operators and any sorting operators as well. So the paging comes quite late on in our query list

//here we check to see is the criteria is not equal to null, it won't be because we know we 've the id
            if (spec.Criteria != null)
            {
                //This line is used for all where() Queries, e.g. For bringing a single product or Filtering products or any thing that uses Where()
                query = query.Where(spec.Criteria); //e.g: query.Where(p => p.productTypeId == id); 
            }

            if (spec.OrederBy != null)
            {
                query = query.OrderBy(spec.OrederBy);
            }

            if (spec.OrederByDescending != null)
            {
                query = query.OrderByDescending(spec.OrederByDescending);
            }
            //so that's our specification set up to accommodate this. And what we want ot do is capture this information from the client and we can do that in the Query string of the Http request

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            //our Includes is gonna be Aggregate because we are combining all of our Include() operations because we may have more than one Include() and we want to accumulate these into an expression
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}