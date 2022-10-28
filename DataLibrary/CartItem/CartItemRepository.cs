using DataLibrary.DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.CartItem
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public CartItemRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }

        public Task<IEnumerable<CartItemModel>> GetCartItems(int id)
        {
            var result = sqlDataAccess.LoadData<CartItemModel, dynamic>("dbo.spCartItem_GetAll", new { CartId = id });
            return result;
        }
        public Task InsertCartItem(CartItemModel cartItem) =>
            sqlDataAccess.SaveData("dbo.spCartItem_Insert", new { cartItem.CartId, cartItem.ProductId });
        public Task UpdateCartItem(CartItemModel cartItem) => sqlDataAccess.SaveData("dbo.spCartItem_Update", new { cartItem.Id, cartItem.ProductId });

        public Task DeleteCartItem(int productId, int amount, int cartId) => sqlDataAccess.SaveData("dbo.spCartItem_Delete", new { ProductId = productId, Amount = amount, CartId = cartId });

    }
}
