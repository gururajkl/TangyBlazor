using Microsoft.AspNetCore.Mvc;
using Tangy_Business.Repository.IRepository;
using Tangy_Models;

namespace TangyWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(productRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest(new ErrorModelDTO
                {
                    ErrorMessage = "Invalid ID please pass correct ID",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            ProductDTO product = productRepository.Get(id.Value);

            if (product == null)
            {
                return BadRequest(new ErrorModelDTO
                {
                    ErrorMessage = "Invalid ID",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(product);
        }
    }
}
