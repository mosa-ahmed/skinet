using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {                       //This generic expression above we that is passed as a parameter we are gonna replace it with an actual expression and it's gonna retuen a boolean
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria {get; }

        //this Includes property is gonna have a list of .Include() statements that we are gonna pass to our .ToListAsync() 
        //we set this property by default to an empty list, list of nothing at the moment but we need to start initially with a list so that we can use it to add things into this list
        public List<Expression<Func<T, object>>> Includes {get; } = new List<Expression<Func<T, object>>>();

        //we will also create a method that will allow us to add include() statements to our list which is a list of expressions

        //protected means that we can access the method that we are about to create in this class itself or any class that derives from this class
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            //so this method will allow us to replace all the Include() statements
            Includes.Add(includeExpression);
        }
    }
}