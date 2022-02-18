using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
    [XmlType("app_preview")]
    public partial class AppPreview
    {
        [XmlAttribute]
        public string display_target { get; set; }

        [XmlAttribute]
        public string position { get; set; }

        [XmlElement("preview_image_time")]
        public PreviewImageTime preview_image_time { get; set; }

        [XmlElement("data_file")]
        public DataFile data_file { get; set; }
    }
}
