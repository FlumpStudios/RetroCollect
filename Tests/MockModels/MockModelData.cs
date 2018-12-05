using ModelData;
using ModelData.Request;
using ModelData.Responses;
using System.Collections.Generic;

namespace Tests.MockModels
{
     class MockModelData
    {
        public static GameListRequest Mock_GameListRequest()
        {
            return new GameListRequest()
            {
                Format = "Jaguar",
                Page = 1,
                SearchText = null,
                ShowClientList = false,
                SortingOptions = null,
                Switchsort = false
            };
        }

        public  static GameListResponse Mock_GameListResponse()
        {
            var gamelist = new List<GameListModel>
            {
                Mock_GameListModel(),
                Mock_GameListModel(),
                Mock_GameListModel()
            };

            var consoleList = new List<string>() {
                "Jaguar",
                "Lynx",
                "Atari 2600"
            };
            return new GameListResponse(gamelist, false, consoleList, false, null, 1, 2, "Jaguar", null, false); 
        }

        public static GameListModel Mock_GameListModel()
        {
            return new GameListModel()
            {
                Developer = "Test Dev",
                Format = "Test Format",
                Genre = "Test Genre",
                Id = 1,
                Name = "test name",
                Publisher = "test publisher",
                ReleaseDateEU = "test eu date",
                ReleaseDateJP = "test jp date",
                ReleaseDateNA = "test na date",
                ScreenShot = null,
                ScreenShotURL = null
            };
        }

        public static ClientListModel Mock_ClientListModel()
        {
            return new ClientListModel()
            {
                GameId = 1,
                Id = 1,
                UserId = "Test User ID"
            };            
        }
    }
}

