using System.Xml.Serialization;

namespace QP.ConfigurationService.Models
{
    [XmlRoot("configuration", IsNullable = false)]
    public class Configuration
    {
        [XmlArray("customers")]
        [XmlArrayItem("customer")]
        public CustomerConfiguration[] Customers { get; set; }

        [XmlArray("app_vars")]
        [XmlArrayItem("app_var")]
        public ApplicationVariable[] Variables { get; set; }
    }
}
