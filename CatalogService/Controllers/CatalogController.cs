using CatalogService.Database;
using CatalogService.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CatalogService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        AppDbContext _db;
        public CatalogController(AppDbContext db)
        {
            _db = db;
        }

        [SwaggerOperation(Summary = "Returning the list of Products", OperationId = "GetProducts")]
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            var model = _db.Products.ToList();
            return model;
        }

        [SwaggerOperation(Summary = "Returning a Product based upon {id}", OperationId = "GetProduct")]
        [HttpGet("{id}")]
        public Product GetProduct(int id)
        {
            Product model = _db.Products.Find(id);
            return model;
        }

        [SwaggerOperation(Summary = "Returning the list of Categories", OperationId = "GetCategories")]
        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            var model = _db.Categories.ToList();
            return model;
        }

        [ProducesResponseType(typeof(Product),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string),StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Adding a new Product based upon passed model", OperationId = "AddProduct")]
        [HttpPost]
        public IActionResult AddProduct(Product model)
        {
            try
            {
                _db.Products.Add(model);
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Update Product By {id}", OperationId = "UpdateProduct")]
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product model)
        {
            try
            {
                if (id != model.ProductId)
                    return BadRequest();

                _db.Products.Update(model);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Delete Product By {id}", OperationId = "DeleteProduct")]
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            Product model = _db.Products.Find(id);
            if (model != null)
            {
                _db.Products.Remove(model);
                _db.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}
