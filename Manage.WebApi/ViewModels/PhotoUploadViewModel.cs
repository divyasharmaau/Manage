using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.ViewModels
{
    public class PhotoUploadViewModel
    {
        public int Id { get; set; }
        public IFormFile Photo { get; set; }
        public string uniqueFileName { get; set; }
    }
}
