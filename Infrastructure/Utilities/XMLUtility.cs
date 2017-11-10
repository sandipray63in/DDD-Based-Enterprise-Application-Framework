using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Infrastructure.Utilities
{
    public static class XMLUtility
    {
        public static string Serialize<TType>(TType obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            StringBuilder result = new StringBuilder();
            using (var writer = XmlWriter.Create(result))
            {
                serializer.Serialize(writer, obj);
            }
            return result.ToString();
        }

        public static TType DeSerialize<TType>(string xml)
        {
            TType desrializedType = default(TType);
            XmlSerializer serializer = new XmlSerializer(typeof(TType));
            using (TextReader reader = new StringReader(xml))
            {
                desrializedType = (TType)serializer.Deserialize(reader);
            }
            return desrializedType;
        }
    }
}
