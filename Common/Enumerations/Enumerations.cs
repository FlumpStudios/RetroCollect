using System.ComponentModel;


namespace Common.Enumerations
{
    public enum GameListColumnNames
    { 
        [Description("Cover Art")]
        Cover = 0,
        [Description("Name")]
        Name = 1,
        [Description("First Release Date")]
        FirstReleaseDate = 2,
        [Description("Rating")]
        Rating = 3,
        [Description("Popularity")]
        Popularity = 4
    }

    public enum FileResponse
    {        
        Success,
        FileNotFound,
        Exception
    }
}
