using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.BlogsModelu
{
    public class BlogsViewModel
    {
        [Required]
        public int? Id { get; set; }//+
        public string Title { get; set; }//+
        public string Body { get; set; }//+
      //  public string Description { get; set; }
        public IFormFile file { get; set; }
        public string fileTemp { get; set; }
    }
}
