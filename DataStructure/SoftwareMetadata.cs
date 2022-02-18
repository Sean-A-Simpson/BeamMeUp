using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
    [XmlType("software_metadata", IncludeInSchema = true)]
    public partial class SoftwareMetadata
    {
        [XmlElement("versions")]
        public Versions versions { get; set; }
    }
}
