﻿using Manage.Application.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manage.Application.Models
{
   public class DepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUserModel> Employees { get; set; }
    }
}
