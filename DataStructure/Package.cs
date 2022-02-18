using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{   [XmlRoot(ElementName ="package", Namespace = "http://apple.com/itunes/importer")]
    public partial class Package
    {
        [XmlAttribute]
        public string version { get; set; }

        public string team_id { get; set; }

        [XmlElement("software")]
        public Software software { get; set; }
    }
}
