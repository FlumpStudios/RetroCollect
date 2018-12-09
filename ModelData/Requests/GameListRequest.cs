using System;

namespace ModelData.Request
{
    public class GameListRequest
    {
        public string SearchText { get; set; }

        public string Platform { get; set; }

        public string SortingOptions { get; set; }

        public bool Switchsort { get; set; }

        public bool ShowClientList { get; set; }        

        public int? Page { get; set; }

        public string ToDate { get; set; }

        public string FromDate { get; set; }

    }
}
