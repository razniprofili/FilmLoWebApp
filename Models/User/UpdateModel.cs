using System;
using System.Collections.Generic;
using System.Text;

namespace Models.User
{
    public class UpdateModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
