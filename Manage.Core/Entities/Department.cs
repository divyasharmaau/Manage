using Manage.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manage.Core.Entities
{
   public class Department 
   {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUser> Employees { get; set; }
    }
}
