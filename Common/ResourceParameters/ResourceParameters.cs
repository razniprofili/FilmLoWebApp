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

        public string OrderBy { get; set; } = "Name"; //stavili smo da je name default za sort
        //ali moramo da pazimo jer je name kod dto objekta,ali ne i entiteta!
        //zato moramo da mapiramo

        //zbog shaping data tj oblikovanja:
        public string Fields { get; set; }
    }
}
