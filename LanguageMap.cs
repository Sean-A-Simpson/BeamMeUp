using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BeamMeUp
{
    [XmlRoot("Languages")]
    public class LanguageMap<TKey, TValue> : Dictionary<TKey, TValue>,
                                              IXmlSerializable
    {
        public XmlSchema GetSchema() { return null; }

        public Platform platform;

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement) { return; }

            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                object key      = reader.GetAttribute("Title");
                object value    = reader.GetAttribute("Value");
                this.Add((TKey)key, (TValue)value);
                reader.Read();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var key in this.Keys)
            {
                writer.WriteStartElement("Language");
                writer.WriteAttributeString("Title", key.ToString());
                writer.WriteAttributeString("Value", this[key].ToString());
                writer.WriteEndElement();
            }
        }

        public static LanguageMap<string, string> GetLanguages(Platform platform)
        {
            
            LanguageMap<string, string> settings    = null;
            string languagesPath                    = Directory.GetCurrentDirectory() + string.Format("\\languages_{0}.xml", platform.ToString().ToLower());
            if (File.Exists(languagesPath))
            {
                try
                {
                    XmlSerializer serializer    = new XmlSerializer(typeof(LanguageMap<string, string>));
                    TextReader textReader       = new StreamReader(languagesPath);
                    settings                    = (LanguageMap<string, string>)serializer.Deserialize(textReader);
                    settings.platform           = platform;
                    textReader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Captain! Failed to read the language map: {0}", e.Message);
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Captain! File languages.xml not found! Is it missing?");
            }
            return settings;
        }
    }
}
