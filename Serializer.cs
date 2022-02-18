using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BeamMeUp
{
    class Serializer
    {
        /// <summary>
        /// populate a class with xml data 
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="input">xml data</param>
        /// <returns>Object Type</returns>
        public T Deserialize<T>(string input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        /// <summary>
        /// convert object to xml string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ObjectToSerialize"></param>
        /// <returns></returns>
        public string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (var ms = new MemoryStream())
            {
                var encoding = Encoding.UTF8;
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = encoding
                };

                using (var xmlTextWriter = XmlWriter.Create(ms, settings))
                {
                    xmlSerializer.Serialize(xmlTextWriter, ObjectToSerialize);
                    string xmlString = encoding.GetString(ms.ToArray());
                    return xmlString;
                }
            }

            //using (StringWriter textWriter = new StringWriter())
            //{
            //    xmlSerializer.Serialize(textWriter, ObjectToSerialize);
            //    return textWriter.ToString();
            //}
        }
    }
}
