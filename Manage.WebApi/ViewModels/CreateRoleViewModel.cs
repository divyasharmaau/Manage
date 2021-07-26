using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.ViewModels
{
    public class CreateRoleViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Role")]
        public string Name { get; set; }
    }
}
