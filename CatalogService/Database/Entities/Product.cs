namespace CatalogService.Database.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitPrice { get; set; }
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
