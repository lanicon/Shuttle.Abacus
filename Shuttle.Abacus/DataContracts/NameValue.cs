using System.Xml.Serialization;

namespace Shuttle.Abacus.DataContracts
{
    public class NameValue
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}