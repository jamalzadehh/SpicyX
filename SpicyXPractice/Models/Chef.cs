using SpicyXPractice.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpicyXPractice.Models
{
    public class Chef:BaseEntity
    {
        public string FullName { get; set; }
        public string Position  { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }    
        public string Fcblink { get; set; }
        public string twtlink { get; set; }
        public string gglink { get; set; }
        public string linkedin { get; set; }

    }
}
