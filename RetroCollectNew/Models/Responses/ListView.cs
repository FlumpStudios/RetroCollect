using ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationLayer.Models.Responses
{

    /// <summary>
    ///Immutable View Model for game list view
    /// </summary>
    public class ListView
    {
        public ListView(IEnumerable<GameListModel> gameListModel, bool isLoggedIn, IEnumerable<string> consoleList, bool reversedList, bool isAdmin)
        {
            GameListModel = gameListModel;
            IsLoggedIn = isLoggedIn;
            ConsoleList = consoleList;
            ReversedList = reversedList;
            IsAdmin = isAdmin;
        }

        public IEnumerable<GameListModel> GameListModel { get; }

        public bool IsLoggedIn { get;  }

        public IEnumerable<string> ConsoleList { get;  }

        public bool ReversedList { get;}

        public bool IsAdmin { get; }
    }
}
