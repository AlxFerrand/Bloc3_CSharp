using Bloc3_CSharp.Services.concretServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Moq;


namespace TestProject_Mercadona
{
    public class SaveFilesServiceTest
    {
        public SaveFilesService saveFilesServiceTest;
        public static string APPLI_IMG_DIRECTORY = "D:\\Codes\\source\\repos\\Bloc3_CSharp\\Bloc3_CSharp";
        public static string TEST_IMG_DIRECTORY = "D:\\Codes\\source\\repos\\Bloc3_CSharp\\TestProject_Mercadona\\imgTest";

        [SetUp]
        public void Setup()
        {
            var hostEnvironment = Mock.Of<IHostEnvironment>();
            Mock.Get(hostEnvironment).Setup(m => m.ContentRootPath).Returns(APPLI_IMG_DIRECTORY);
            saveFilesServiceTest = new SaveFilesService(hostEnvironment);
        }

        public static IFormFile AsMockIFormFile (FileInfo physicalFile)
        {
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream ();
            var writter = new StreamWriter (ms);
            writter.Write(physicalFile.OpenRead());
            writter.Flush();
            ms.Position = 0;
            var fileName = physicalFile.Name;

            fileMock.Setup(m => m.FileName).Returns(fileName);
            fileMock.Setup(m => m.Length).Returns(ms.Length);
            fileMock.Setup(m => m.OpenReadStream()).Returns(ms);
            fileMock.Setup(m => m.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));
            
            return fileMock.Object;
        }

        [Test]
        public void CheckMimeTypeImgTest_With_TxtFile()
        {
            var physicalFile = new FileInfo(TEST_IMG_DIRECTORY + "\\UploadsFiles.txt");
            IFormFile formFile = AsMockIFormFile(physicalFile);
            Assert.False(saveFilesServiceTest.CheckMimeTypeImg(formFile));
        }

        [Test]
        public void CheckMimeTypeImgTest_With_JpgFile()
        {
            var physicalFile = new FileInfo(TEST_IMG_DIRECTORY + "\\TestImage.jpg");
            IFormFile formFile = AsMockIFormFile(physicalFile);
            Assert.True(saveFilesServiceTest.CheckMimeTypeImg(formFile));
        }

        [Test]
        public void SaveFileToImgDirectoryTest_With_JpgFile()
        {
            var physicalFile = new FileInfo(TEST_IMG_DIRECTORY + "\\TestImage.jpg");
            IFormFile formFile = AsMockIFormFile(physicalFile);
            saveFilesServiceTest.SaveFileToImgDirectory(formFile, "SaveFileToImgDirectoryTest");
            string[] files = Directory.GetFiles(APPLI_IMG_DIRECTORY +"\\wwwroot\\img");
            bool fileFind = false;
            foreach (string file in files)
            {
                if (file.Contains("SaveFileToImgDirectoryTest")){
                    fileFind = true;
                    saveFilesServiceTest.DeleteFileToImgDirectory(file);
                }
            }
            Assert.True(fileFind);
        }

        [Test]
        public void SaveFileToImgDirectoryTest_With_TxtFile()
        {
            var physicalFile = new FileInfo(TEST_IMG_DIRECTORY + "\\UploadsFiles.txt");
            IFormFile formFile = AsMockIFormFile(physicalFile);
            string result = saveFilesServiceTest.SaveFileToImgDirectory(formFile, "SaveFileToImgDirectoryTest");
            Assert.AreEqual("Error : MIME Type not good", result);
        }

        [Test]
        public void SaveFileToImgDirectoryTest_With_EmptyFile()
        {
            var emptyFileMock = new Mock<IFormFile>();
            emptyFileMock.Setup(m => m.FileName).Returns("emptyFileMock.jpg");
            emptyFileMock.Setup(m => m.Length).Returns(0);

            string result = saveFilesServiceTest.SaveFileToImgDirectory(emptyFileMock.Object, "SaveFileToImgDirectoryTest");
            Assert.AreEqual("Error : Empty file", result);
        }

    }
}
