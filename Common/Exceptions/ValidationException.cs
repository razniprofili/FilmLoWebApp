using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(string message) : base(message)
        {

        }
    }
}
