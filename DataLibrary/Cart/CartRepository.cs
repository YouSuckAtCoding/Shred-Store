using DataLibrary.DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Cart
{
    public class CartRepository : ICartRepository
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public CartRepository(ISqlDataAccess _sqlDataAccess)
        {
            sqlDataAccess = _sqlDataAccess;
        }

        public async Task<CartModel?> GetCart(int id)
        {
            var result = await sqlDataAccess.LoadData<CartModel, dynamic>("dbo.spCart_GetById", new { Id = id });

            return result.FirstOrDefault();

        }

        public Task InsertCart(CartModel cart) =>
            sqlDataAccess.SaveData("dbo.spCart_Insert", new {cart.UserId, cart.CreatedDate });

        public Task DeleteCart(int id) => sqlDataAccess.SaveData("dbo.spCart_Delete", new { Id = id });
    }
}
