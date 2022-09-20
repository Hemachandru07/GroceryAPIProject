using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GroceryAPI.Models
{
    public class Grocery
    {
        [Key]
        [Display(Name = "Grocery ID")]
        public int GroceryID { get; set; }
        [Display(Name = "Grocery Name")]
        public string? GroceryName { get; set; }
        public float? Price { get; set; }
        public int? Stock { get; set; }
        public List<Cart>? cart { get; set; }
        public List<Receipt>? receipt { get; set; }
    }
}
