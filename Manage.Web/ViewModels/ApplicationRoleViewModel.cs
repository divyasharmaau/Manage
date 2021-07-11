using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.ViewModels
{
    public class ApplicationRoleViewModel
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public IList<string> Users { get; set; }
    }
}
