using System.Collections.Generic;
using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
    [XmlType("versions", IncludeInSchema = true)]
    public partial class Versions
    {
        [XmlElement("version")]
        public List<Version> versions1 { get; set; }
    }
}
