using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GroceryAPI.Models
{
    public class Admin
    {
        [Key]
        [Display(Name = "Admin ID")]
        public int AdminID { get; set; }

        
        [Display(Name = "Admin Name")]
        public string? AdminName { get; set; }

        
        [Display(Name = "Email-ID")]
        [DataType(DataType.EmailAddress)]
        public string? EmailID { get; set; }

        
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
          ErrorMessage = "Password must contains one Uppercase,one Lowercase and one Specialcharacter")]
        public string? Password { get; set; }
    }
}
