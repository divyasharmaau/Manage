using Manage.WebApi.Interface;
using Manage.WebApi.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Utilities
{
    public  class UploadImageHelper : IUploadImageHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UploadImageHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public PhotoUploadViewModel UploadImage(PhotoUploadViewModel model ) 
        {
            var uniqueFileName = "";
            //to get to the path of the wwwwrootfolder
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/img");
            //append GUID value  and undersacore for unique File Name
            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            model.uniqueFileName = uniqueFileName;
            //copy file to images folder
            model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            return (model);
        }

        public void UpdateEmployeePersonalDetailsPhoto(EmployeePersonalDetailsViewModel model)
        {
            if (model == null ||
                   string.IsNullOrEmpty(model.ApiPhotoPath))
            {
                return;
            }

            var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/img",
             model.ApiPhotoPath);
            if (!System.IO.File.Exists(photoPath))
            {
                model.ApiPhotoPath = null;
                return;
            }

            var photoBytes = System.IO.File.ReadAllBytes(photoPath);

            var fileExtension = model.ApiPhotoPath.Split('.')[1];

            model.ApiPhotoPath =
                $"data:image/{fileExtension};base64,{Convert.ToBase64String(photoBytes)}";

        }
    }
}
