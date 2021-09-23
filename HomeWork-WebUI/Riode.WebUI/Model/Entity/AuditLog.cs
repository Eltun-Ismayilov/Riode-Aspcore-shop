using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class AuditLog:BaseEntity
    {

        public string Pati { get; set; } // unvan Tamadi gosderir.

        public bool IsHttps { get; set; } //sorgunun https olub olmadigni yoxluyuruq

        public string QueryString { get; set; } //Pati beraber hansi queryString gelib bize onu goturmeliyik

        public string Method { get; set; } // Sorgunun GET veya POST oldugnu yoxluyaq.

        public string Area { get; set; } //Area nedi.

        public string Controller { get; set; } // Controller nedi.

        public string Action { get; set; }  // Action nedi.

        public int StatusCode { get; set; } //Status Codun ne oldugnu gotureciyik

        public DateTime RequestTime { get; set; } //Sorgunun tarixi.

        public DateTime ResponseTime { get; set; } // Sorgunun cvb tarixi.









    }
}
