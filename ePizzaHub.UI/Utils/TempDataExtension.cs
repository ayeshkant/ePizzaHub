using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json;

namespace ePizzaHub.UI.Utils
{
    public static class TempDataExtension
    {
        public static void Set<T>(this ITempDataDictionary tempData, string key, T value)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            tempData[key] = JsonSerializer.Serialize(value, options);
        }
        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object value = tempData.Peek(key);
            if (value != null)
                return JsonSerializer.Deserialize<T>((string)value);
            return null;
        }
    }
}
