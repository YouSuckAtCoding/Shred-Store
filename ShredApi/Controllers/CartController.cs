using DataLibrary.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ShredApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository cartRepository;

        public CartController(ICartRepository _cartRepository)
        {
            cartRepository = _cartRepository;
        }
        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartModel>> Get(int id)
        {
            var cart = await cartRepository.GetCart(id);
            return Ok(cart);
        }
        [HttpPost]
        public async Task<ActionResult<CartModel>> Post([FromBody] CartModel cart)
        {
            if (ModelState.IsValid)
            {
                await cartRepository.InsertCart(cart);
                return Ok(cart);
            }
            return BadRequest(cart);
        }
        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await cartRepository.DeleteCart(id);
            return Ok();
        }

    }
}
