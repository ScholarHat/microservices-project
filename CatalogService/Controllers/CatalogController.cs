using CatalogService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        ICatalogServiceRepository _catalogService;
        public CatalogController(ICatalogServiceRepository catalogService)
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

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _catalogService.GetCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _catalogService.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
    }
}
