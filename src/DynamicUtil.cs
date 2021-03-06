﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="exclude">Comma split property names to be exclude from mapping</param>
        /// <param name="mapping">Key - model property name; Value - body property name</param>
        /// <returns>Property names which were updated</returns>
        public static List<string> UpdateModel(object model, dynamic body, string exclude = null, Dictionary<string, string> mapping = null)
        {
            var updated = new List<string>();
            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                var propName = propertyInfo.Name;
                if (exclude != null && exclude.Split(',').ToList().Contains(propName))
                    continue;
                string bodyPropertyName;
                if (mapping != null)
                {
                    if (!mapping.TryGetValue(propName, out bodyPropertyName))
                        continue;
                }
                else
                    bodyPropertyName = propName;
                if (HasProperty(body, bodyPropertyName))
                {
                    ((JsonElement)body).TryGetProperty(bodyPropertyName, out var value);
                    if (value.ValueKind == JsonValueKind.Null)
                    {
                        propertyInfo.SetValue(model, null);
                    }
                    else
                    {
                        string stringValue = value.ToString();
                        if (propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IConvertible)))
                            propertyInfo.SetValue(model, Convert.ChangeType(stringValue, propertyInfo.PropertyType));
                        else
                            propertyInfo.SetValue(model, JsonConvert.DeserializeObject(stringValue, propertyInfo.PropertyType));
                    }
                    updated.Add(propName);
                }
            }
            return updated;
        }

        /// <summary>
        /// Method checks if given dynamic object has specific property
        /// </summary>
        /// <param name="body">Dynamic objec</param>
        /// <param name="property">Property name</param>
        /// <returns>True if given object has specific property</returns>
        public static bool HasProperty(dynamic body, string property)
        {
            return ((JsonElement)body).TryGetProperty(property, out _);
        }

        public static string GetString(dynamic body, string property)
        {
            ((JsonElement)body).TryGetProperty(property, out var result);
            return result.GetString();
        }
    }
}
