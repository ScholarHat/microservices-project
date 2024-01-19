using CatalogService.Database.Entities;
using CatalogService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ICatalogServiceRepository _catalogService;
        public ProductController(ICatalogServiceRepository catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _catalogService.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _catalogService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            try
            {
                _catalogService.AddProduct(product);
                return CreatedAtAction("AddProduct", product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateProduct(Product product)
        {
            try
            {
                _catalogService.UpdateProduct(product);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
