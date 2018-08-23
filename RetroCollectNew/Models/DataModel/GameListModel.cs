using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RetroCollectNew.Models.DataModel
{
    public class GameListModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(70)]
        
        public String Format { get; set; }
        [MaxLength(70)]

        public String Name { get; set; }
        [MaxLength(70)]
        [Required]
        public string Developer { get; set; }
        [MaxLength(70)]
        [Required]
        public string Genre { get; set; }
        [MaxLength(70)]
        [Required]
        public string Publisher { get; set; }
        [MaxLength(70)]
        public string ReleaseDateNA { get; set; }
        [MaxLength(70)]
        public string ReleaseDateEU { get; set; }
        [MaxLength(70)]
        public string ReleaseDateJP { get; set; }    
    }
}
