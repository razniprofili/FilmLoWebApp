using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public interface IPropertyCheckerService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
