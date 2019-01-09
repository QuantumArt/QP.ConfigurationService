using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QP.ConfigurationService.Models
{
    public class CustomerConfiguration
    {
        [XmlAttribute("customer_name")]
        public string Name { get; set; }

        [XmlAttribute("exclude_from_schedulers")]
        public bool ExcludeFromSchedulers { get; set; }

        [XmlElement("db")]
        public string ConnectionString { get; set; }
    }
}
