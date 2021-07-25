using Manage.Web.Interface;
using Manage.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Utilities
{
    public class FileUploadHelper : IFileUploadHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadHelper( IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public FileUploadViewModel UploadFile(FileUploadViewModel model)
        {
            string uniqueFileName;
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "dist/files");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            FileStream fs = new FileStream(filePath, FileMode.Create);
            model.File.CopyTo(fs);
            model.UniqueFileName = uniqueFileName;
            return model;
        }
    }
}
