using System.ComponentModel.DataAnnotations;

namespace sd.Jatek.Web.Models
{
    public class GameViewModel
    {
        [Required]
        [Range(1, 20, ErrorMessage = "Number of rounds must be between 1 and 20")]
        public int Rounds { get; set; }
        public bool Public { get; set; }
    }
}
