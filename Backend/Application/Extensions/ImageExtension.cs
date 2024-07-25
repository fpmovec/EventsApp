using Microsoft.AspNetCore.Http;
using Web.DTO;

namespace Application.Extensions
{
    public static class ImageExtension
    {
        private const string PathToImageFolder = @"images";
        public static async Task<ImageInfo> AddImageAsync(this IFormFile image, string webRootPath, CancellationToken cancellationToken)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string filePath = Path.Combine(webRootPath, PathToImageFolder);

            await using (FileStream stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
            {
                await image.CopyToAsync(stream, cancellationToken);
            }

            return new(fileName, Path.Combine(PathToImageFolder, fileName));
        }
    }
}
