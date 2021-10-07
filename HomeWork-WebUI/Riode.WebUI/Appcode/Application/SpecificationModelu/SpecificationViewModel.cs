using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.SpecificationModelu
{
    public class SpecificationViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
