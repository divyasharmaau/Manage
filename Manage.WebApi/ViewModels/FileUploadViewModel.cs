using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.ViewModels
{
    public class FileUploadViewModel
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }
        public string UniqueFileName { get; set; }
    }
}
