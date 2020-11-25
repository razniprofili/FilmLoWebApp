using System;
using System.Collections.Generic;
using System.Text;

namespace Models.User
{
    public class UserModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Picture { get; set; }
    }
}
