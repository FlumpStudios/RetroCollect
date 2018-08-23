using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetroCollectNew.Models.DataModel
{
    public class ClientListModel
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string UserId { get; set; }

        //TODO: This should really be a foreign key            
        public int GameId { get; set; }
    }
}
