using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Infrastructure.Utilities
{
    public class CloningUtility
    {
        public static TType Clone<TType>(TType obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (TType)formatter.Deserialize(ms);
            }
        }
    }
}
