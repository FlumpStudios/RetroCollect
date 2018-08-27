namespace ApplicationLayer.Models.Request
{
    public class GameListRequest
    {
        public string SearchText { get; set; }

        public string Format { get; set; }

        public string SortingOptions { get; set; }

        public bool Switchsort { get; set; }

        public bool ShowClientList { get; set; }        
    }
}
