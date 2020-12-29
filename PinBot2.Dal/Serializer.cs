using PinBot2.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PinBot2.Dal
{
    public class Serializer
    {
        public static string SerializeObject<T>(T obj)
        {

            MemoryStream ms = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(ms, obj);
            return Convert.ToBase64String(ms.ToArray());
            /*
            MemoryStream msTestString = new MemoryStream();
            ProtoBuf.Serializer.Serialize<T>(msTestString, obj);
            string stringBase64 = Convert.ToBase64String(msTestString.ToArray());
            return stringBase64;
            */
        }

        public static object DeSerializeObject<T>(string str)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(str);
                MemoryStream ms = new MemoryStream(bytes);
                BinaryFormatter b = new BinaryFormatter();
                var d = b.Deserialize(ms);
                return d;
            }
            finally //catch
            {
                // byte[] bytes = Convert.FromBase64String(str);
                // MemoryStream ms = new MemoryStream(bytes); //new stream coz old one already used
                // var b = ProtoBuf.Serializer.Deserialize<T>(ms);
                // return b;
            }

        }


    }

}
