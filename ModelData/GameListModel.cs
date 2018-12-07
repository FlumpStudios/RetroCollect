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
        
        public string Platform { get; set; }
        [MaxLength(70)]

        public string Name { get; set; }

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

        [NotMapped]
        public long Rating { get; set; }

        [NotMapped]
        public string First_release_date { get; set; }

        [NotMapped]
        public Cover Cover { get; set; }
    }

    public class Cover
    {
        public string Url { get; set; }

        public string Cloudinary_id { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }
    }
}
