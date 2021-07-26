using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }
        public int Id { get; set; }

        public string UserId { get; set;}
        public string UserName { get; set;}
        public IList<string> Roles { get; set; }
        public IList<string> Claims { get; set; }
    }
}
