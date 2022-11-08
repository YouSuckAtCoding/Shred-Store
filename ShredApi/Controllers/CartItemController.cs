using DataLibrary.CartItem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ShredApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemRepository cartItemRepository;

        public CartItemController(ICartItemRepository _cartItemRepository)
        {
            cartItemRepository = _cartItemRepository;
        }
        // GET: api/<CartItemController>
        [HttpGet("{cartId}")]
        public async Task<ActionResult<IEnumerable<CartItemModel>>> Get(int cartId)
        {
            var cartItems = await cartItemRepository.GetCartItems(cartId);
            return Ok(cartItems);
        }
        // POST api/<CartItemController>
        [HttpPost]
        public async Task<ActionResult<CartItemModel>> Post([FromBody] CartItemModel cartItem)
        {
            if (ModelState.IsValid)
            {
                await cartItemRepository.InsertCartItem(cartItem);
                return Ok(cartItem);
            }
            return BadRequest(cartItem);
        }

        // PUT api/<CartItemController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CartItemModel>> Put(int id, [FromBody] CartItemModel cartItem)
        {
            if (id != cartItem.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await cartItemRepository.UpdateCartItem(cartItem);
                return Ok(cartItem);
            }
            return BadRequest();
        }

        // DELETE api/<CartItemController>/5
        [HttpDelete("{productId}/{amount}/{cartId}")]
        public async Task<ActionResult> Delete(int productId, int amount, int cartId)
        {
            await cartItemRepository.DeleteCartItem(productId, amount, cartId);
            return Ok();
        }

        [HttpDelete("{cartId}")]
        public async Task<ActionResult> DeleteAll(int cartId)
        {
            await cartItemRepository.DeleteAllCartItem(cartId);
            return Ok();
        }
    }
}

