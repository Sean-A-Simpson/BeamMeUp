using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
    [XmlType("version", IncludeInSchema = true)]
    public partial class Version
    {
        [XmlAttribute(AttributeName = "string")]
        public string string_string { get; set; }

        [XmlElement("locales")]
        public Locales locales { get; set; }
    }
}
