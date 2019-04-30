using Newtonsoft.Json;
using System;

namespace Dms.Product.General.ThridTool
{
    public class JsonService
    {
        public string Serialize<T>(T t) where T : class
        {
            if (t == null)  return string.Empty;

            return JsonConvert.SerializeObject(t, Formatting.Indented);
        }

        public T DeSerialize<T>(string jsonStr) where T : class
        {
            if (string.IsNullOrEmpty(jsonStr))
                return null;
            try
            {
                var instance = JsonConvert.DeserializeObject<T>(jsonStr);

                return instance;
            }
            catch
            {
                return null;
            }

        }

    }
}
