﻿using Bloc3_CSharp.Services.abstractServices;

namespace Bloc3_CSharp.Services.concretServices
{
    public class SaveFilesService : ISaveFilesService
    {
        private readonly IHostEnvironment _hostEnvironment;
        public SaveFilesService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        public string SaveFileToImgDirectory(IFormFile file, string newFileName)
        {
            string uploadPath = Path.Combine(_hostEnvironment.ContentRootPath,"wwwroot" ,"img");
            if (file.Length > 0)
            {
                if (CheckMimeTypeImg(file))
                {
                    newFileName = newFileName + Path.GetExtension(file.FileName).ToLowerInvariant();
                    string filePath = Path.Combine(uploadPath, newFileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    return newFileName;
                }
                return "Error : MIME Type not good";
            }
            return "Error : Empty file";
        }

        public string SaveFileToMyDirectory(string path, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public bool CheckMimeTypeImg (IFormFile file)
        {
            using (var reader = new BinaryReader(file.OpenReadStream()))
            {
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                var signature = MimeDictionary.mimeDictionaryImg[ext];
                var headerBytes = reader.ReadBytes(signature.Max(m => m.Length));
                return signature.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(headerBytes));
            }
        }
    }
}