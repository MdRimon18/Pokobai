//using Microsoft.AspNetCore.Components.Forms;

//namespace Pms.Data.Repository.Shared
//{
//    public class FileUploadService  
//    {
//        public async Task<string> UploadFileAsync(IBrowserFile file, string urlPath)
//        {
//            if (file == null || file.Size == 0)
//            {
//                throw new ArgumentException("No file uploaded.");
//            }
//            string extension = Path.GetExtension(file.Name);
//            string fileName = GetFileName(extension);
//            string monthDate = DateTime.UtcNow.ToString("MMMM-yyyy");
//            string customUrl = Path.Combine("wwwroot", urlPath, monthDate);
//            string fileUrl = Path.Combine(urlPath, monthDate, fileName);

//            string basePath = Path.Combine(Directory.GetCurrentDirectory(), customUrl);
//            string destinationPath = Path.Combine(basePath, fileName);

//            if (!Directory.Exists(basePath))
//            {
//                Directory.CreateDirectory(basePath);
//            }

//            using (var stream = new FileStream(destinationPath, FileMode.Create))
//            {
//                await file.OpenReadStream().CopyToAsync(stream);
//            }

//            return $"/{fileUrl.Replace("\\", "/")}";
//        }
//        private string GetFileName(string extension)
//        {
//            return $"{Guid.NewGuid()}{extension}";
//        }
//    }
//}
