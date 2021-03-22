using System;
using System.Collections.Generic;
using System.Text;

namespace BDProject.Models
{
    public class User
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }

        public string token { get; set; }
    }
}
