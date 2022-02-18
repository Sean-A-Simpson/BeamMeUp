using System.Collections.Generic;
using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
    [XmlType("locale", IncludeInSchema = true)]
    public partial class Locale
    {
        [XmlAttribute]
        public string name { get; set; }

        public string title { get; set; }
        public string subtitle { get; set; }

        public string description { get; set; }
        
        [XmlArray("keywords")]
        [XmlArrayItem(ElementName="keyword")]
        public List<string> keywords { get; set; }

        public string version_whats_new { get; set; }

        [XmlElement("app_previews")]
        public AppPreviews app_previews { get; set; }

        [XmlElement("software_screenshots")]
        public SoftwareScreenshots software_screenshots { get; set; }
    }
}
