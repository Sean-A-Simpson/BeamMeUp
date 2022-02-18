using System.Collections.Generic;
using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
    [XmlType("software_screenshots")]
    public partial class SoftwareScreenshots
    {
        [XmlElement("software_screenshot")]
        public List<SoftwareScreenshot> SoftwareScreenshots1 { get; set; }
    }
}
