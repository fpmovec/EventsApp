using Microsoft.AspNetCore.Http;
using Web.ViewModels;

namespace Application.Extensions
{
    public static class ImageExtension
    {
        private const string PathToImageFolder = @"images";
        public static async Task<ImageInfo> AddImageAsync(this IFormFile image, string webRootPath)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string filePath = Path.Combine(webRootPath, PathToImageFolder);

            await using (FileStream stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return new(fileName, Path.Combine(PathToImageFolder, fileName));
        }
    }
}
