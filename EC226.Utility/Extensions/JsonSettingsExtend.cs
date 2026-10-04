using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace EC226.Utility.Extensions
{
   public static class JsonSettingsExtend
   {
      public static JsonSerializerSettings GetDefaultSettings()
      {
         return new JsonSerializerSettings
         {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
         };
      }
   }
}
