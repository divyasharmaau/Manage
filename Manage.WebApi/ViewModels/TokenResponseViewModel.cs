using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.ViewModels
{
        [JsonObject(MemberSerialization.OptOut)]
        public class TokenResponseViewModel
        {
            #region Constructor
            public TokenResponseViewModel()
            {

            }
            #endregion

            #region Properties
            public string token { get; set; }
            public int expiration { get; set; }
            public string refresh_token { get; set; }
            public string role_name { get; set; }
        
            #endregion
        }
}
