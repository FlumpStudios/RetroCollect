using ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Enumerations;
using Common.Extensions;
using System.Collections;
using Common.Helpers;

namespace ModelData.Responses
{

    /// <summary>
    ///Immutable View Model for game list view
    /// </summary>
    public class GameListResponse
    {
        public GameListResponse(
            IEnumerable<GameListModel> gameListModel, 
            bool isLoggedIn,
            Dictionary<string, string> consoleList, 
            bool isAdmin, 
            string currentOrderBy, 
            int? page, 
            int lastPage,
            string format,
            string sortingOptions,       
            bool showClientList,
            string toDate,
            string fromDate)
        {
            GameListModel = gameListModel;
            IsLoggedIn = isLoggedIn;
            ConsoleList = consoleList;
            IsAdmin = isAdmin;
            ColumnNames = EnumerationsHelper.GetDescriptionList<GameListColumnNames>() as IEnumerable<string>;
            CurrentOrderBy = currentOrderBy;
            Page = page;
            LastPage = lastPage;
            Platform = format;
            SortingOptions = sortingOptions;
            ShowClientList = showClientList;
            FromDate = fromDate;
            ToDate = toDate;

        }

        public IEnumerable<GameListModel> GameListModel { get; }

        public bool IsLoggedIn { get;  }

        public Dictionary<string,string> ConsoleList { get; }
        
        public bool IsAdmin { get; }

        public IEnumerable<string> ColumnNames { get; }

        public string CurrentOrderBy { get; }

        public int? Page { get; }

        public int LastPage { get;}
                
        public string Platform { get; }

        public string SortingOptions { get;  }
       
        public bool ShowClientList { get;  }

        public string FromDate { get; }

        public string ToDate { get; }

    

    }
}
