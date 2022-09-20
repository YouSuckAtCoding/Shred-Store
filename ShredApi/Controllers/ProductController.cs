using DataLibrary.Product;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShredApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository _productRepository)
        {
            productRepository = _productRepository;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Get()
        {
            var products = await productRepository.GetProducts();
            return Ok(products);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> Get(int id)
        {
            var product = productRepository.GetProduct(id);
            return Ok(product);
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<ProductModel>> Post([FromBody] ProductModel product)
        {
            if (ModelState.IsValid)
            {
                await productRepository.InsertProduct(product);
                return Ok(product);
            }
            return BadRequest(product);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductModel>> Put(int id, [FromBody] ProductModel product)
        {
            if(id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await productRepository.UpdateProduct(product);
                return Ok(product);
            }
            return BadRequest();
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await productRepository.DeletProduct(id);
            return Ok();
        }
    }
}
