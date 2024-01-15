using System.ComponentModel.DataAnnotations;

namespace sd.Jatek.Web.Models
{
    public class GameViewModel
    {
        [Required]
        public int Rounds { get; set; }
        public bool Public { get; set; }
    }
}
