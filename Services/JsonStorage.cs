using System;
using System.IO;
using System.Web.Script.Serialization;

namespace chat.Services
{
    public class JsonStorage
    {
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

        public T Load<T>(string path, T fallback)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return fallback;
                }

                string json = File.ReadAllText(path);
                T result = serializer.Deserialize<T>(json);
                return result == null ? fallback : result;
            }
            catch
            {
                return fallback;
            }
        }

        public void Save<T>(string path, T data)
        {
            string folder = Path.GetDirectoryName(path);

            if (!string.IsNullOrWhiteSpace(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string json = serializer.Serialize(data);
            File.WriteAllText(path, json);
        }

        public string Serialize(object data)
        {
            return serializer.Serialize(data);
        }

        public object DeserializeDynamic(string json)
        {
            return serializer.DeserializeObject(json);
        }
    }
}
