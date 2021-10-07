using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.OneCategoryModelu
{


    public class CategoryViewModel
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
