using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class AuditLog:BaseEntity
    {

        public string Pati { get; set; } // Unvan Tam Adi gosderir.

        public bool IsHttps { get; set; } //Sorgunun HTTPS(HTTP) olub olmadigni yoxluyuruq

        public string QueryString { get; set; } //Pati beraber hansi QueryString gelib bize onu goturmeliyik

        public string Method { get; set; } // Sorgunun GET veya POST oldugnu yoxluyaq.

        public string Area { get; set; } //Area nedi.

        public string Controller { get; set; } // Controller nedi.

        public string Action { get; set; }  // Action nedi.

        public int StatusCode { get; set; } //Status Codun ne oldugnu gotureciyik YENI UGURLU OLDUGNU(200) UGURSUZ OLDUGNU(404)

        public DateTime RequestTime { get; set; } //Sorgu gedir

        public DateTime ResponseTime { get; set; } // Sorgunun cvb tarixi.









    }
}
