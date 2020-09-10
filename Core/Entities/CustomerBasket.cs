using System.Collections.Generic;

namespace Core.Entities
{
    public class CustomerBasket
    {
        public CustomerBasket()     //this constructor is for that we can create a new instance without needing to pass id
        {
        }

        public CustomerBasket(string id)
        {
            Id = id;    //since we are creating a new list when we create a new instance of this class, we don't need a parameter of List<BasketItem> items in the constructor()
        }

        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>(); 
    }
}