using Api.Infrastructure.Services;
using Api.Models.Products;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("/GetProducts", Name = "GetProducts")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("/GetProduct/{id}", Name = "GetProduct")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("/AddProduct", Name = "AddProduct")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            await _productService.AddProductAsync(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("/UpdateProduct/{id}", Name = "UpdateProduct")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpDelete("/DeleteProduct/{id}", Name = "DeleteProduct")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
