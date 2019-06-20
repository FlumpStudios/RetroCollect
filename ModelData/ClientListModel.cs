using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModelData
{
    public class ClientListModel
    {
        public ClientListModel()
        {
            ClientGame = new HashSet<ClientGame>();
        }
      
        [Key]
        public int ClientId { get; set; }

        [MaxLength(50)]
        public string UserId { get; set; }
        
        public ICollection<ClientGame> ClientGame { get; set; }
    }

    public class ClientGame
    {
        public ClientGame()
        {
            GameFormat = new HashSet<GameFormat>();
        }

        [Key]
        public int ClientGameId { get; set; }

        [MaxLength(50)]
        public string GameId { get; set; }

        public ICollection<GameFormat> GameFormat { get; set; }
    }

    public class GameFormat
    {
        [Key]
        public int GameFomatId { get; set; }

        public int ClientGameId { get; set; }

        public string IgdbKey { get; set; }
    }
}
