using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        public string ReleaseDateNA { get; set; }

        public string ReleaseDateEU { get; set; }

        public string ReleaseDateJP { get; set; }

        public  IEnumerable<IFormFile> ScreenShot { get; set; }

        public IEnumerable<string> ScreenShotURL { get; set; }

        public long Rating { get; set; }

        [DisplayName("First Release Date")]
        public string First_release_date { get; set; }

        public Cover Cover { get; set; }

        public long Popularity { get; set; }

        public IEnumerable<string> Platforms { get; set; }

        public string Summary { get; set; }

        public IEnumerable<Screenshots> Screenshots { get; set; }

        public IEnumerable<Videos> Videos { get; set; }

        public IEnumerable<Artworks> Artworks { get; set; }

      
    }


    public class Artworks
    {
        public string Url { get; set; }
        public string Cloudinary_id { get; set; }
    }
    public class Videos
    {
        public string Name { get; set; }
        public string Video_id { get; set; }
    }

    public class Screenshots
    {   
        public string Url { get; set; }
        public string Cloudinary_id { get; set; }

    }

    public class Cover
    {
        public string Url { get; set; }

        public string Cloudinary_id { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }
    }
}
