using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi.ViewModels
{
     [JsonObject(MemberSerialization.OptOut)]
        public class TokenRequestViewModel
        {
            #region Constructor
            public TokenRequestViewModel()
            {

            }
            #endregion

            #region Properties
            public string grant_type { get; set; }
            public string client_id { get; set; }
            public string client_secret { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string refresh_token { get; set; }
            public string role_name { get; set; }

       
            
        #endregion
    }  
}
