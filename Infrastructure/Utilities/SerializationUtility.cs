using System.IO;
using System.Xml.Serialization;

namespace Infrastructure.Utilities
{
    public static class SerializationUtility
    {
        public static void Serialize<TSerializable>(string filePath)
        {
            var serializer = new XmlSerializer(typeof(TSerializable));
            var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, default(TSerializable));
            writer.Close();
        }

        public static TSerializable Deserialize<TSerializable>(string filePath)
            where TSerializable : class
        {
            var serializer = new XmlSerializer(typeof(TSerializable));
            var fileStream = new FileStream(filePath, FileMode.Open);
            return serializer.Deserialize(fileStream) as TSerializable;
        }
    }
}
