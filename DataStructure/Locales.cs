using System.Collections.Generic;
using System.Xml.Serialization;

namespace BeamMeUp.DataStructure
{
    [XmlType("locales", IncludeInSchema = true)]
    public partial class Locales
    {
        [XmlElement("locale")]
        public List<Locale> locales1 { get; set; }

        public Locale AddLocale(string name)
        {
            Locale newLocale = new Locale();
            newLocale.name = name;
            locales1.Add(newLocale);
            return newLocale;
        }
    }
}
