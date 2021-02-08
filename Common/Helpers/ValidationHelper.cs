
using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helpers
{
    public static class ValidationHelper
    {

        public static void ValidateNotNull<T>(T entity) where T : class
        {
            if (entity == null)
                throw new ValidationException($"{typeof(T).Name} not exist!");
        }


        public static void ValidateEntityExists<T>(T entity) where T : class
        {
            if (entity != null)
                throw new ValidationException($"{typeof(T).Name} currently exists!");
        }
    }
}
