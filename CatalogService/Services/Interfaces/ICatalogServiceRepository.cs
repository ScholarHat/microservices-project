using CatalogService.Database.Entities;

namespace CatalogService.Services.Interfaces
{
    public interface ICatalogServiceRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int id);
        IEnumerable<Category> GetCategories();
        Category GetCategory(int id);

        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
