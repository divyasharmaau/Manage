using Manage.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Interface
{
    public interface IFileUploadHelper
    {
        FileUploadViewModel UploadFile(FileUploadViewModel model);
    }
}
