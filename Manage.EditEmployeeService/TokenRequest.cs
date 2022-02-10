using System;
using System.Collections.Generic;
using System.Text;

namespace Manage.EditEmployeeService
{
    class TokenRequest
    {       
        public string username { get; set; }
        public string password { get; set; }
        public string client_id { get; set; }
        public string grant_type { get; set; }
        public string role_name { get; set; }
        public string refresh_token { get; set; }       

    }
}
