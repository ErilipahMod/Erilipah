using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace Erilipah.Core
{
    public static class ConfigReader
    {
        private static JToken config;

        [AutoInit(InitHooks.Load | InitHooks.Unload)]
        private static readonly Dictionary<string, object> cache = new Dictionary<string, object>();

        [HookLoading(LoadHooks.Load)]
        private static void OnLoad()
        {
            Stream file = Erilipah.Instance.GetFileStream("Core/Config.json");
            config = JToken.ReadFrom(new JsonTextReader(new StreamReader(file)));
        }

        public static T Get<T>(string path)
        {
            if (cache.TryGetValue(path, out object ret))
            {
                return (T)ret;
            }
            JToken token = config;
            foreach (var prop in path.Split('.'))
            {
                token = token[prop];
            }
            ret = token.ToObject<T>();
            cache[path] = ret;
            return (T)ret;
        }
    }
}