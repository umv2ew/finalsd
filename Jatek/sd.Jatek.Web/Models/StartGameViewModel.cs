using System.ComponentModel.DataAnnotations;

namespace sd.Jatek.Web.Models
{
    public class StartGameViewModel
    {
        [Required(ErrorMessage = "Egy kör számot meg kell adni")]
        [Range(1, 10, ErrorMessage = "Csak 1 és 10 közötti kör lehetséges")]
        public int Rounds { get; set; }
        public bool Public { get; set; }
    }
}
