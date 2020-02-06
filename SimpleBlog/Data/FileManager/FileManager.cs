using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBlog.Data.FileManager
{
    public class FileManager : IFileManager
    {
        private string _imagesPath;

        public FileManager(IConfiguration config)
        {
            _imagesPath = config["Path:Images"];
        }

        public FileStream ImageStream(string image)
        {
            return new FileStream(Path.Combine(_imagesPath, image), 
                FileMode.Open, FileAccess.Read);
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            try
            {
                var savePath = Path.Combine(_imagesPath);
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                var date = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
                var filename = $"img_{date}{mime}";

                using (var fileStream = new FileStream(
                    Path.Combine(savePath, filename), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return filename;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
        }
    }
}
