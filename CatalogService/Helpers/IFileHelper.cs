namespace CatalogService.Helpers
{
    public interface IFileHelper
    {
        void DeleteFile(string imageUrl);
        string UploadFile(IFormFile file);
    }
}
