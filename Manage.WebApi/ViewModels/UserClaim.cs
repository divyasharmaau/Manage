using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.ViewModels
{
    public class UserClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }
}
