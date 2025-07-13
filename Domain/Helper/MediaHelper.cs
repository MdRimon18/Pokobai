using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helper
{
    public class MediaHelper
    {

        public async Task<byte[]> GetBytes(IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public static string UploadOriginalFile(IFormFile file, string urlPath)
        {
            var image = Image.FromStream(file.OpenReadStream());
            var resized = new Bitmap(image);
            using var imageStream = new MemoryStream();
            var extension = GetExtension(file.FileName);
            if (extension == "png")
            {
                resized.Save(imageStream, ImageFormat.Png);
            }
            else
            {
                resized.Save(imageStream, ImageFormat.Jpeg);
            }
            
          
            var imageBytes = imageStream.ToArray();
            return UploadFile(imageBytes, urlPath);
        }
      
        public static string UploadLargeFile(IFormFile sourceImage, string urlPath)
        {
            string filename = string.Empty;
            System.Drawing.Image souimage =
                System.Drawing.Image.FromStream(sourceImage.OpenReadStream());
            var image = ResizeImageOriginalRatio(souimage, 600, 400);
            filename = UploadFile(image, urlPath);

            return filename;
        }

        public static string UploadMediumFile(IFormFile sourceImage, string urlPath)
        {
            string filename = string.Empty;
            System.Drawing.Image souimage =
                System.Drawing.Image.FromStream(sourceImage.OpenReadStream());
            var image = ResizeImageOriginalRatio(souimage, 368, 349);
            filename = UploadFile(image, urlPath);
            return filename;
        }

       
        public static string UploadSmallFile(IFormFile sourceImage, string urlPath)
        {
            string fileName = string.Empty;
            System.Drawing.Image souimage =
                System.Drawing.Image.FromStream(sourceImage.OpenReadStream());
           
            var image = ResizeImageOriginalRatio(souimage, 208, 183);
            fileName = UploadFile(image, urlPath);
            return fileName;
        }

        //public static string UploadOriginalfileV2()
        //{
        //    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
        //    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
        //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        model.ProfileImage.CopyTo(fileStream);
        //    }
        //}

        public static string UploadFile(byte[] imageBytes, string urlPath)
        {

            string filename = string.Empty;
            filename = GetFileName();
            string MonthDate = DateTime.UtcNow.ToString("MMMM-yyyy");
            string customUrl = "wwwroot" + urlPath + "/" + MonthDate + "/";
            string fileUrl = urlPath + "/" + MonthDate + "/";
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), customUrl);
            var path = Path.Combine(Directory.GetCurrentDirectory(), customUrl, filename);
            if (!Directory.Exists(basePath))
            {
                DirectoryInfo di = Directory.CreateDirectory(basePath);
            }
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, 4096))
            {
                stream.Write(imageBytes, 0, imageBytes.Length);
            }
            filename = fileUrl + filename;
            return filename;
        }


        //public static string UploadAllFile(IFormFile sourceImage, string urlPath)
        //{
        //    string fileName = string.Empty;
        //    System.Drawing.Image souimage =
        //        System.Drawing.Image.FromStream(sourceImage.OpenReadStream());
        //    var image = ResizeImageOriginalRatio(souimage, 208, 183);
        //    fileName = UploadAnyFile(souimage, urlPath);
        //    return fileName;
        //}

        public static string UploadAnyFile(byte[] imageBytes, string urlPath,string extention)
        {
            string filetype = GetExtension(extention);
            string filename = string.Empty;

            filename = GetFileName(filetype);
            string MonthDate = DateTime.UtcNow.ToString("MMMM-yyyy");
            string customUrl = "wwwroot" + urlPath + "/" + MonthDate + "/";
            string fileUrl = urlPath + "/" + MonthDate + "/";
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), customUrl);
            var path = Path.Combine(Directory.GetCurrentDirectory(), customUrl, filename);
            if (!Directory.Exists(basePath))
            {
                DirectoryInfo di = Directory.CreateDirectory(basePath);
            }
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, 4096))
            {
                stream.Write(imageBytes, 0, imageBytes.Length);
            }
            filename = fileUrl + filename;
            return filename;
        }
        public static byte[] ResizeImageOriginalRatio(Image image, int width, int height)
        {           
            var resized = new Bitmap(image, new Size(width, height));
            using var imageStream = new MemoryStream();
            resized.Save(imageStream, ImageFormat.Jpeg);
            var imageBytes = imageStream.ToArray();
            return imageBytes;
        }
        private static string GetFileName()
        {
            string extension = ".jpg";
            string fileName = Path.ChangeExtension(
                Path.GetRandomFileName(),
                extension
            );
            return fileName;
        }
        private static string GetFileName(string extension)
        {
            string fileName = Path.ChangeExtension(
                Path.GetRandomFileName(),
                extension
            );
            return fileName;
        }
        public static string GetExtension(string attachment_name)
        {
            var index_point = attachment_name.IndexOf(".") + 1;
            return attachment_name.Substring(index_point);
        }
    }
}
