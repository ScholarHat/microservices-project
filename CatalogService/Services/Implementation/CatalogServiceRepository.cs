using CatalogService.Database;
using CatalogService.Database.Entities;
using CatalogService.Services.Interfaces;

namespace CatalogService.Services.Implementation
{
    public class CatalogServiceRepository : ICatalogServiceRepository
    {
        AppDbContext _db;
        IConfiguration _configuration;
        public CatalogServiceRepository(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;

        }

        public void AddProduct(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            Product product = _db.Products.Find(id);
            _db.Products.Remove(product);
            _db.SaveChanges();
        }

        public IEnumerable<Category> GetCategories()
        {
            return _db.Categories;
        }

        public Category GetCategory(int id)
        {
            return _db.Categories.Find(id);
        }

        public Product GetProduct(int id)
        {
            return _db.Products.Find(id);
        }

        public IEnumerable<Product> GetProducts()
        {
            IList<Product> products = new List<Product>();
            var data = _db.Products.ToList();

            foreach (var item in data)
            {
                Product product = new Product
                {
                    ProductId = item.ProductId,
                    Name = item.Name,
                    Description = item.Description,
                    UnitPrice = item.UnitPrice,
                    ImageUrl = _configuration["ImageServer"]+ item.ImageUrl,
                    CategoryId = item.CategoryId,
                    CreatedDate = item.CreatedDate
                };
                products.Add(product);
            }
            return products;
        }

        public void UpdateProduct(Product product)
        {
            _db.Products.Update(product);
            _db.SaveChanges();
        }
    }
}
