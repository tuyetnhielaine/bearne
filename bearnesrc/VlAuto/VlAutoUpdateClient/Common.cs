using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VlAutoUpdateClient;

public class Common
{
    //địa chỉ máy chủ.
    public static string Domain = "http://192.168.1.110";

    public static string GetUrl(string path)
    {
        return $"{Domain}{path}";
    }
    public static string JsonSerializeObject<T>(T obj)
    {
        if (obj == null)
        {
            return string.Empty;
        }

        return JsonConvert.SerializeObject(obj, Formatting.None,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "yyyy-MM-ddTHH:mm:ss",
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
    }

    public static T? JsonDeserializeObject<T>(string? json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return default(T);
        }

        try
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
            };
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
        catch (Exception)
        {
            return default(T);
        }
    }

}
