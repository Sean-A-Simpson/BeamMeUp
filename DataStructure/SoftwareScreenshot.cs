using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
    [XmlType("software_screenshot")]
    public partial class SoftwareScreenshot
    {
        [XmlAttribute]
        public string display_target { get; set; }

        [XmlAttribute]
        public string position { get; set; }

        public string file_name { get; set; }
        public string size { get; set; }

        [XmlElement("checksum")]
        public Checksum checksum { get; set; }
    }
}
