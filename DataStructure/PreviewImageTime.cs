using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
    [XmlType("preview_image_time")]
    public partial class PreviewImageTime
    {
        [XmlAttribute]
        public string format { get; set; }

        [XmlText]
        public string text { get; set; }
    }
}
