namespace Bloc3_CSharp.Services.abstractServices
{
    public interface ISaveFilesService
    {
        public string SaveFileToMyDirectory(string path, IFormFile file);
        public string SaveFileToImgDirectory(IFormFile file, string newFileName);
        public void DeleteFileToImgDirectory(string fileName);
    }
}
