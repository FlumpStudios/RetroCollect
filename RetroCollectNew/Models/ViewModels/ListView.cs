using RetroCollectNew.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetroCollectNew.Models.ViewModels
{
    public class ListView
    {
        public IEnumerable<GameListModel> GameListModel { get; set; }

        public bool IsLoggedIn { get; set; }

        public IEnumerable<string> ConsoleList { get; set; }

        public bool ReversedList { get; set; }

        public bool IsAdmin { get; set; }
    }
}
