using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ResourceParameters
{
    public class ResourceParameters
    {
        //za pretragu
        public string SearchQuery { get; set; }

        //stranicenje
        const int maxPageSize = 20;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public string OrderBy { get; set; } = "Name";

        public string Fields { get; set; }
    }
}
