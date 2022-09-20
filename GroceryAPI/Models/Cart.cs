using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GroceryAPI.Models
{
    public class Cart
    {
        [Key]
        [Display(Name = "Cart ID")]
        public int CartID { get; set; }

        [Display(Name = "Cart Type ID")]
       
        public string? CartTypeId { get; set; }
       
        public int GroceryID { get; set; }
        [ForeignKey("GroceryID")]
        public virtual Grocery? grocery { get; set; }
        [Required(ErrorMessage = "*")]
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer? customer { get; set; }
       
        public int Quantity { get; set; }
       
        [Display(Name = "Unit Price")]
        public float? UnitPrice { get; set; }
    } 
       
}
