using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GroceryAPI.Models
{
    public class Customer
    {
        [Key]
        [Display(Name = "Customer ID")]
        public int CustomerID { get; set; }

        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Numbers and special characters are not allowed")]
        [Display(Name = "Customer Name")]
        public string? CustomerName { get; set; }
       
        [Display(Name = "Email-ID")]
        [DataType(DataType.EmailAddress)]
        public string? CustomerEmail { get; set; }
     
        [Display(Name = "Mobile Number")]
        public long MobileNumber { get; set; }
       
        public string? Address { get; set; }
        
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
           ErrorMessage = "Password must contains one Uppercase,one Lowercase and one Specialcharacter")]
        public string? Password { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage = "Password Not Matching")]
        [Display(Name = "Confirm Password")]
        public string? CPassword { get; set; }

        [Display(Name = "Cart Type ID")]
        public string? CartTypeId { get; set; }
        public List<Cart>? cart { get; set; }
        public List<Payment>? Payment { get; set; }
        public List<Receipt>? Receipt { get; set; }
    }
}
