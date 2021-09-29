using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.ProductSizeModelu
{
    public class SizeViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string Description { get; set; }
    }
}
