using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.ProductColorModelu
{
    public class ColorViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string Description { get; set; }
    }
}
