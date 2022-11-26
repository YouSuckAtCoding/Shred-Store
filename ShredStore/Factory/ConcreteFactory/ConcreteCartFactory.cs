using ShredStore.Factory.Interface;
using ShredStore.Models;

namespace ShredStore.Factory.ConcreteFactory
{
    public class ConcreteCartFactory : ICartFactory
    {
        public CartViewModel CreateCart(int userId)
        {
            CartViewModel newCart = new CartViewModel();
            newCart.UserId = userId;
            newCart.CreatedDate = DateTime.Now;
            return newCart;
        }
    }
}
