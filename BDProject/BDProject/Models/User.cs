using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
