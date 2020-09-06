using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
         //we create here Generic methods
         //this method (Criteria) is equal to Where() 
         Expression<Func<T, bool>> Criteria {get; }

         //this method (Includes) is equal to Include()
         List<Expression<Func<T, object>>> Includes {get; }

        //After this we replace what we have here Expression<Func<T, bool>> | List<Expression<Func<T, object>>> we replace all of this with actual expressions
        //So we will use a BaseSpecification class that is gonna implement these interface methods 

        //Until now, Here we have two options: 1- we can get sth by some sort of criteria And 2- we can include navigation properties  

        Expression<Func<T, object>> OrederBy {get; }
        Expression<Func<T, object>> OrederByDescending {get; }

        int Take {get;}
        int Skip {get;}
        bool IsPagingEnabled {get;}
    }
}