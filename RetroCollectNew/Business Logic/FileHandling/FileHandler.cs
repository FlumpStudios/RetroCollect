using ApplicationLayer.Enumerations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationLayer.Business_Logic.FileHandling
{   
    public class FileHandler : IFileHandler
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileHandler(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void SaveFile(string folder, IEnumerable<IFormFile> files)
        {
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, folder);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    //Check if folder exists, if it dones't make one.
                    string fileName = Path.GetFileName(file.FileName);
                    bool exists = Directory.Exists(uploads);                 
                    if (!exists) Directory.CreateDirectory(uploads);

                    var filePath = Path.Combine(uploads, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }
        }

        public IEnumerable<string> LoadFiles(string folderName)
        {            
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
            if (!Directory.Exists(uploads)) return null;

            List<string> filesInFolder = new List<string>();
            foreach (string s in Directory.GetFiles(uploads).Select(Path.GetFileName))
            {
                filesInFolder.Add(Path.Combine(folderName,s));
            }
            return filesInFolder;
        }

        public FileResponse DeleteFile(string fileLocation)
        {
            var fullPath = _hostingEnvironment.WebRootPath + fileLocation;
            try
            {
                if (!File.Exists(fullPath)) return FileResponse.FileNotFound;
                File.Delete(fullPath);
            }
            catch (Exception e)
            {
                //TODO: Log exception
                return FileResponse.Exception;               
            }
            return FileResponse.Success;
        }
        
    }
}
