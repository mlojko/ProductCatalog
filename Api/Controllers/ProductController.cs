using Api.Infrastructure.Services;
using Api.Models.Products;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0", Deprecated = true)]
    public class ProductController(IProductService productService) : Controller
    {
        private readonly IProductService _productService = productService;

        [HttpGet("GetProducts", Name = "GetProducts")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("GetPagedProducts", Name = "GetPagedProducts")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetProducts(int page)
        {
            var products = await _productService.GetProductsAsync(page);
            return Ok(products);
        }

        [HttpGet("GetProducts", Name = "GetProductsV2")]        
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetProductsV2()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("GetProduct/{id}", Name = "GetProduct")]
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

        [HttpPost("AddProduct", Name = "AddProduct")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromBody] ProductPayload product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock

            };
            await _productService.AddProductAsync(newProduct);
            return CreatedAtRoute("GetProduct", new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("UpdateProduct/{id}", Name = "UpdateProduct")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpDelete("DeleteProduct/{id}", Name = "DeleteProduct")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
