using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Product
{
    public class ProductAddModel
    {
     
        public int CategoryID { get; set; }
        [Required(ErrorMessage ="Please Enter ProductName")]
        [StringLength(400,ErrorMessage ="must be between 3 to 400",MinimumLength =3)]
        public string ProductName { get; set; }
        [Range(0,2000000000,ErrorMessage ="Price Must be positive number")]
        public int BasePrice { get; set; }
        public string Desceription { get; set; }
        public string Introduction { get; set; }
        public string DefaultImage { get; set; }
        public int SupplierID { get; set; }
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$",
        //ErrorMessage = "رمز عبور باید حداقل ۸ کاراکتر، شامل حداقل یک حرف بزرگ، یک حرف کوچک، یک عدد و یک کاراکتر خاص باشد.")]
        //public string Password { get; set; }
        //[Compare("Password",ErrorMessage ="both password entery must be same")]
        //public string RePassword { get; set; }
    }
}
