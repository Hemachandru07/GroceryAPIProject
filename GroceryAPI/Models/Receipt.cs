using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GroceryAPI.Models
{
    public class Receipt
    {
        [Key]
        public int ReceiptID { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReceiptDate { get; set; }
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer? customer { get; set; }
        public int GroceryID { get; set; }
        [ForeignKey("GroceryID")]
        public virtual Grocery? grocery { get; set; }
        public int? Quantity { get; set; }

        public float? Amount { get; set; }
    }
}
