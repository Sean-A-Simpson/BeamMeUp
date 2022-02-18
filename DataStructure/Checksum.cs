using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
    [XmlType("checksum")]
    public partial class Checksum
    {
        [XmlAttribute]
        public string type { get; set; }

        [XmlText]
        public string text { get; set; }
    }
}
