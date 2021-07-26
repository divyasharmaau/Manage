﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Manage.WebApi.ViewModels
{
   public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUserViewModel> Employees { get; set; }

    }
}
