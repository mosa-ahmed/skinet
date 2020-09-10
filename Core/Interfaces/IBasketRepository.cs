using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IBasketRepository  //we have created a specific repository for our baskets. We can't use our generic repository We created earlier on because that one was very specific for entity framework and we're not using entity framework for our basket. We're using Redis instead. And we're going to interact directly with the Redis database from our repository. And we're not going to use entity framework to send our queries to and from Redis.
    {
         Task<CustomerBasket> GetBasketAsync(string basketId);
         Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
         Task<bool> DeleteBasketAsync(string basketId);
    }
}