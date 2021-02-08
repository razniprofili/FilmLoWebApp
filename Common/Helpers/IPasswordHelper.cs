using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helpers
{
   public interface IPasswordHelper
    {
        public bool ValidatePassword(string password, string correctHash);
        public string CreateHash(string password);
    }
}
