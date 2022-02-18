using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
    [XmlType("software", IncludeInSchema = true)]
    public partial class Software
    {
        public string vendor_id { get; set; }

        [XmlElement("software_metadata")]
        public SoftwareMetadata software_metadata { get; set; }
    }
}
