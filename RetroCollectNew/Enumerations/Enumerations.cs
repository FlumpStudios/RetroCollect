using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationLayer.Enumerations
{
    public enum GameListColumnNames
    { 
        [Description("Name")]
        Name = 0,
        [Description("Developer")]
        Developer = 1,
        [Description("Genre")]
        Genre = 2,
        [Description("Publisher")]
        Publisher = 4
    }

    public enum FileResponse
    {        
        Success,
        FileNotFound,
        Exception
    }
}
