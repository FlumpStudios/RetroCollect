using System.Collections.Generic;
using ApplicationLayer.Enumerations;
using Microsoft.AspNetCore.Http;

namespace ApplicationLayer.Business_Logic.FileHandling
{
    public interface IFileHandler
    {
        void SaveFile(string folder, IEnumerable<IFormFile> files);
        IEnumerable<string> LoadFiles(string folderName);
        FileResponse DeleteFile(string fileLocation);

    }
}