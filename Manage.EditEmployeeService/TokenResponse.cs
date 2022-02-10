using System;
using System.Collections.Generic;
using System.Text;

namespace Manage.EditEmployeeService
{

    public class TokenResponse
    {
        public string token { get; set; }
        public int expiration { get; set; }
        public string refresh_token { get; set; }
        public string role_name { get; set; }
        public string userId { get; set; }
        public string profile_picture_path { get; set; }
        public string user_name { get; set; }
    }

}
