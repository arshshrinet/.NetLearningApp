using LearningApp.Model;
using LearningApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LearningApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            var productResponse = await productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), productResponse);
        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        public async Task<ActionResult<Product>> GetProducts()
        {
            Console.WriteLine($"Start Time: {DateTime.Now}");
            var result = await productService.GetProductAsync();
            Console.WriteLine($"End Time: {DateTime.Now}");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var productResponse = await productService.GetProductByIdAsync(id);
            if (productResponse is null)
            {
                return BadRequest("Product is not Present.");
            }
            return Ok(productResponse);
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            var result = await productService.UpdateProductAsync(product);
            if (result is null)
            {
                return BadRequest("Product is not Present.");
            }
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteProduct(Guid Id)
        {
            var result = await productService.DeleteProductByIdAsync(Id);
            if (!result)
            {
                return BadRequest("Failed To Delete Product.");
            }
            return Ok(result);
        }
    }
}
