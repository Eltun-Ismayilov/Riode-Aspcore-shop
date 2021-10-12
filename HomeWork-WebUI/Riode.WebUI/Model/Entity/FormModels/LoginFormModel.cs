using System.ComponentModel.DataAnnotations;

namespace Riode.WebUI.Model.Entity.FormModels
{
    public class LoginFormModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
