using Manage.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.Interface
{
   public interface IUploadImageHelper
    {
         PhotoUploadViewModel UploadImage(PhotoUploadViewModel model);
    }
}
