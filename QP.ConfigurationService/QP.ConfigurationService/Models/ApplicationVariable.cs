using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QP.ConfigurationService.Models
{
    public class ApplicationVariable
    {
        [XmlAttribute("app_var_name")]
        public string Name { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
