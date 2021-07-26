using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.ViewModels
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        public IList<UserClaim> Claims { get; set; }
    }
}
