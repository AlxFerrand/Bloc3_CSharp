﻿using Bloc3_CSharp.Services.abstractServices;
using System.Text.RegularExpressions;

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
            newFileName = Regex.Replace(newFileName,"[^a-zA-Z0-9_]","");
            string uploadPath = Path.Combine(_hostEnvironment.ContentRootPath,"wwwroot" ,"img");
            if (file.Length > 0)
            {
                try
                {
                    if (CheckMimeTypeImg(file))
                    {
                        newFileName = newFileName + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + Path.GetExtension(file.FileName).ToLowerInvariant();
                        string filePath = Path.Combine(uploadPath, newFileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        return newFileName;
                    }
                }
                catch (Exception ex) {
                    return "Error : MIME Type not good";
                }  
                return "Error : MIME Type not good";
            }
            return "Error : Empty file";
        }

        public string SaveFileToMyDirectory(string path, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public void DeleteFileToImgDirectory(string fileName) 
        {
            if (!fileName.Equals(""))
            {
                string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "img", fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        public bool CheckMimeTypeImg (IFormFile file)
        {
            using (var reader = new BinaryReader(file.OpenReadStream()))
            {
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (MimeDictionary.mimeDictionaryImg.ContainsKey(ext))
                {
                    var signature = MimeDictionary.mimeDictionaryImg[ext];
                    var headerBytes = reader.ReadBytes(signature.Max(m => m.Length));
                    return signature.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(headerBytes));
                }
                return false;
                
            }
        }
    }
}
