using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity.FormModels
{
    public class RegisterFormModel
    {
     

        [Required]

        public string UserName { get; set; }

        [Required]
        [EmailAddress]

        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare("Password")]
        public string CheckPassword { get; set; }
    }
}
