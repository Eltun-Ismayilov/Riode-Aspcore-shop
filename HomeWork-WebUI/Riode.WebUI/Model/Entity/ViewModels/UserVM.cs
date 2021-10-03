using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool Activated { get; set; }
        public string Role { get; set; }

        public List<string> Roles { get; set; }
    }
}
