using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RetroCollectNew.Enumerations;

using RetroCollectNew.Extensions.EnumExtensions;

namespace RetroCollectNew.Models.Requests
{
    public class GameListRequestModel
    {
        public string SearchText { get; set; }

        public string Format { get; set; }

        public string SortingOptions { get; set; }

        public bool Switchsort { get; set; }

        public bool ShowClientList { get; set; }

        public OrderByOptions OrderByOptions;
    }
}
