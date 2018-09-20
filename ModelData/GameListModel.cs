using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelData
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

        [NotMapped]
        public  IEnumerable<IFormFile> ScreenShot { get; set; }

        [NotMapped]
        public IEnumerable<string> ScreenShotURL { get; set; }

   
    }
}
