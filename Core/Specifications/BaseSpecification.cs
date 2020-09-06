using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    //THIS CLASS IS JUST A SETTER TO THE PROPERTIES OF THE INTERFACE. JUST A SETTER, NOTHING ELSE
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {                       //This generic expression above we that is passed as a parameter we are gonna replace it with an actual expression and it's gonna retuen a boolean
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        //this Includes property is gonna have a list of .Include() statements that we are gonna pass to our .ToListAsync() 
        //we set this property by default to an empty list, list of nothing at the moment but we need to start initially with a list so that we can use it to add things into this list
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrederBy { get; private set; } //we will add the ability to set what this is inside this particular class. we add the getter and the setter in here

        public Expression<Func<T, object>> OrederByDescending { get; private set; } //we will add the ability to set what this is inside this particular class. we add the getter and the setter in here

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        //we will also create a method that will allow us to add include() statements to our list which is a list of expressions

        //protected means that we can access the method that we are about to create in this class itself or any class that derives from this class
        //We are creating these methods to set the properties of the interface : ISpecification
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            //so this method will allow us to replace all the Include() statements
            Includes.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrederBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrederByDescending = orderByDescExpression;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true; //we make use of this property inside our Specification evaluator so that we can know whether or not to page the results
        }

        //Now these methods need to be evaluated by our Specification evaluators. We then get added to IQueryable that we return and pass to our method that we that is going to call list .TolList()
        //So now we will go to the SpecificationEvaluator and check to see if we have an OrderBy in our specification or an OrderByDescending

    }
}