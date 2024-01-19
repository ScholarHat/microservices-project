using CatalogService.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        IFileHelper _fileHelper;
        public FileController(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        //only for one file
        //[HttpPost]
        //public IActionResult UploadFile(IFormFile file)
        //{
        //    var imageUrl = _fileHelper.UploadFile(file);
        //    return Ok(imageUrl);
        //}

        //for multiple files
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var formCollection = Request.ReadFormAsync().Result;
                var file = formCollection.Files.First();

                var dbPath = _fileHelper.UploadFile(file);
                return Ok(new { dbPath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("{imageUrl}")]
        public IActionResult DeleteFile(string imageUrl)
        {
            _fileHelper.DeleteFile(imageUrl);
            return Ok();
        }   
    }
}
