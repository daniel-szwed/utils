using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace ArbreSoft.Utils
{
    public static class DynamicUtil
    {
        /// <summary>
        /// When property names are match model will be updated
        /// </summary>
        /// <param name="model">Model instance to update</param>
        /// <param name="body">Dynamic contains some properties</param>
        /// <param name="excluded">Property name to be excluded</param>
        /// <returns>Lists of properties which have been updated or not recognized</returns>
        public static UpdateResult UpdateModel(object model, dynamic body, params string[] excluded)
        {
            var result = new UpdateResult();
            var modelProperties = model.GetType().GetProperties();
            var jsonProperties = ((JsonElement)body).EnumerateObject().ToList();

            foreach (var jsonProperty in jsonProperties)
            {
                if (excluded.Contains(jsonProperty.Name))
                {
                    continue;
                }

                if (modelProperties.TryGet(jsonProperty.Name, out var modelPropertyInfo))
                {
                    if (jsonProperty.Value.ValueKind == JsonValueKind.Null)
                    {
                        modelPropertyInfo.SetValue(model, null);
                    }
                    else
                    {
                        model.SetProperty(modelPropertyInfo, jsonProperty);
                    }

                    result.Updated.Add(modelPropertyInfo.Name);
                }
                else
                {
                    result.NotRecognized.Add(jsonProperty.Name);
                }
            }

            return result;
        }

        /// <summary>
        /// Method checks if given dynamic object has specific property
        /// </summary>
        /// <param name="body">Dynamic objec</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>True if given object has specific property</returns>
        public static bool JsonHasProperty(dynamic body, string propertyName)
        {
            return ((JsonElement)body).EnumerateObject()
                        .Any(property => string.Compare(
                            property.Name,
                            propertyName,
                            StringComparison.OrdinalIgnoreCase) == 0);
        }

        public static string GetString(dynamic body, string propertyName)
        {
            var result = ((JsonElement)body).EnumerateObject()
                        .Where(property => string.Compare(
                            property.Name,
                            propertyName,
                            StringComparison.OrdinalIgnoreCase) == 0)
                        .FirstOrDefault();

            return result.Value.GetString();
        }
    }

    public class UpdateResult
    {
        public List<string> Updated { get; }
        public List<string> NotRecognized { get; }

        public UpdateResult()
        {
            Updated = new();
            NotRecognized = new();
        }
    }

    static class Extensions
    {
        public static bool TryGet(this PropertyInfo[] propertyInfoArray, string propertyName, out PropertyInfo propertyInfo)
        {
            propertyInfo = propertyInfoArray.Where(property => string.Compare(
                    property.Name,
                    propertyName,
                    StringComparison.OrdinalIgnoreCase) == 0)
                .FirstOrDefault();

            return propertyInfo is not null;
        }

        public static bool Contains(this string[] excluded, string propertyName) => 
            excluded.Any(item => string.Compare(
                item,
                propertyName,
                StringComparison.OrdinalIgnoreCase) == 0);

        public static void SetProperty(this object model, PropertyInfo modelPropertyInfo, JsonProperty jsonProperty)
        {
            if (modelPropertyInfo.PropertyType.GetInterfaces().Contains(typeof(IConvertible)))
            {
                modelPropertyInfo.SetValue(model, Convert.ChangeType(jsonProperty.Value.ToString(), modelPropertyInfo.PropertyType));
            }
            else
            {
                modelPropertyInfo.SetValue(model, JsonSerializer.Deserialize(jsonProperty.Value.ToString(), modelPropertyInfo.PropertyType));
            }
        }
    }
}
