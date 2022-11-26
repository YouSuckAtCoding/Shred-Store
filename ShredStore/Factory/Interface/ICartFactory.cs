using ShredStore.Models;

namespace ShredStore.Factory.Interface
{
    public interface ICartFactory
    {
        CartViewModel CreateCart(int userId);
    }
}
