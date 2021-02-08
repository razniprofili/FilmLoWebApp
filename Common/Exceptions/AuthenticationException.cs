using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    public class AuthenticationException : ApplicationException
    {
        public AuthenticationException(string message) : base(message)
        {

        }
    }
}
