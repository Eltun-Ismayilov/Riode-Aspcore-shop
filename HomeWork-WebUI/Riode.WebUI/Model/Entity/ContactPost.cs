using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class ContactPost:BaseEntity
    {
        [Required] // bos ola bilmez not null  bularin name Annotations adlanir
        public string Name { get; set; }

        [Required]
        [EmailAddress] // ancaq email yaza bilsiner  amma database dusmur front yoxlanilir

        public string Email { get; set; }

        [Required]

        public string Comment { get; set; }

        public string Answer { get; set; } // kim terefinden cvb verilib

        public DateTime? AnswerdData { get; set; } // kim terefinden ne vaxt cvb verilib

        public int? AnswerByUserId { get; set; } // hansi id terefinden  kim terefinden cvb veriib


    }
}
