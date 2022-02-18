using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
    [XmlType("data_file")]
    public partial class DataFile
    {
        [XmlAttribute]
        public string role { get; set; }

        public string file_name { get; set; }
        public string size { get; set; }

        [XmlElement("checksum")]
        public Checksum checksum { get; set; }
    }
}
