using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QP.ConfigurationService.Models
{
    [XmlRootAttribute("configuration", IsNullable = false)]
    public class Configuration
    {
        [XmlArray("customers")]
        [XmlArrayItem("customer")]
        public CustomerConfiguration[] Customers { get; set; }
    }
}
