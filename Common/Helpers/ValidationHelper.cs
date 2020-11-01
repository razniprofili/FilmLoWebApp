
using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helpers
{
    public static class ValidationHelper
    {
        // na ovaj nacin metoda prima sve vrste klasa
        // genericka je
        public static void ValidateNotNull<T>(T entity) where T : class
        {
            if (entity == null)
                throw new ValidationException($"{typeof(T).Name} ne postoji!"); //ako je objekat null, naziv tog entiteta ne postoji!
        }

        public static void ValidateEntityExists<T>(T entity) where T : class
        {
            if (entity != null)
                throw new ValidationException($"{typeof(T).Name} vec postoji!");
        }
    }
}
