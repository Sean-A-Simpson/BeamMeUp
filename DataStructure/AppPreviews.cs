using System.Collections.Generic;
using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
	[XmlType("app_previews")]
	public partial class AppPreviews
	{
		[XmlElement("app_preview")]
		public List<AppPreview> AppPreviews1 { get; set; }
	}
}

