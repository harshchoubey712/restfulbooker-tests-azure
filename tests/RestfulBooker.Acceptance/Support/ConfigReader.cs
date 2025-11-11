using System.IO;
using Newtonsoft.Json.Linq;

namespace RestfulBooker.Acceptance.Support
{
  public static class ConfigReader
  {
    private static readonly JObject config;

    static ConfigReader()
    {
      var jsonText = File.ReadAllText("appsettings.json");
      config = JObject.Parse(jsonText);
    }

    public static string Get(string key)
    {
      return config[key]?.ToString() ?? "";
    }
  }
}
