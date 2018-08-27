using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModelData
{
    public class ClientListModel
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string UserId { get; set; }
        
        public int GameId { get; set; }
    }
}
