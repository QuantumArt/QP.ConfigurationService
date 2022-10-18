using System;
using System.Xml.Serialization;

namespace QP.ConfigurationService.Models
{
    public class CustomerConfiguration
    {
        [XmlAttribute("customer_name")]
        public string Name { get; set; }

        [XmlAttribute("exclude_from_schedulers")]
        public bool ExcludeFromSchedulers { get; set; }
        
        [XmlIgnore]
        public DatabaseType DbType { get; set; }

        [XmlAttribute("db_type")]
        public string TypeXml
        {
            get { return DbType.ToString().ToLowerInvariant() ; }
            set
            {
                if (Enum.TryParse<DatabaseType>(value, true, out var parsed))
                {
                    DbType = parsed;
                }
            }
        }      

        [XmlElement("db")]
        public string ConnectionString { get; set; }
    }
}
