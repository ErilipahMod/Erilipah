using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Erilipah.Core
{
    public static class ConfigReader
    {
        private static JToken config;

        [HookLoading(LoadHooks.Load)]
        private static void OnLoad()
        {
            Stream file = Erilipah.Instance.GetFileStream("Core/Config.json");
            config = JToken.ReadFrom(new JsonTextReader(new StreamReader(file)));
        }

        public static T Get<T>(string path)
        {
            JToken token = config;
            foreach (var prop in path.Split('.'))
            {
                token = token[prop];
            }
            return token.ToObject<T>();
        }
    }
}