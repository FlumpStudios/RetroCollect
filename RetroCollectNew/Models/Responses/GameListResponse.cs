using ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.Helpers;
using ApplicationLayer.Enumerations;
using System.Collections;

namespace ApplicationLayer.Models.Responses
{

    /// <summary>
    ///Immutable View Model for game list view
    /// </summary>
    public class GameListResponse
    {
        public GameListResponse(IEnumerable<GameListModel> gameListModel, 
            bool isLoggedIn, 
            IEnumerable<string> consoleList, 
            bool isAdmin, 
            string currentOrderBy, 
            int? page, 
            int lastPage,
            string format,
            string sortingOptions,       
            bool showClientList)
        {
            GameListModel = gameListModel;
            IsLoggedIn = isLoggedIn;
            ConsoleList = consoleList;
            IsAdmin = isAdmin;
            ColumnNames = EnumerationsHelper.GetDescriptionList<GameListColumnNames>() as IEnumerable<string>;
            CurrentOrderBy = currentOrderBy;
            Page = page;
            LastPage = lastPage;
            Format = format;
            SortingOptions = sortingOptions;
            ShowClientList = showClientList;

        }

        public IEnumerable<GameListModel> GameListModel { get; }

        public bool IsLoggedIn { get;  }

        public IEnumerable<string> ConsoleList { get; }
        
        public bool IsAdmin { get; }

        public IEnumerable<string> ColumnNames { get; }

        public string CurrentOrderBy { get; }

        public int? Page { get; }

        public int LastPage { get;}
                
        public string Format { get; }

        public string SortingOptions { get;  }
       
        public bool ShowClientList { get;  }

    }
}
