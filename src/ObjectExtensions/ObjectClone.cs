using System;

namespace ArbreSoft.Utils.ObjectExtensions
{
    public static class ObjectExtensions
    {
        // TODO: extend this method for reference type, arrays etc.
        public static T Clone<T>(this T source) where T : class, new()
        {
            if (source == null)
            {
                return source;
            }

            var result = new T();
            var properties = source.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(source, null);

                if (value is null)
                {
                    continue;
                }

                var type = value.GetType(); 
                if (type.IsValueType)
                {
                    property.SetValue(result, value, null);
                }
                if (type.FullName == "System.String")
                {
                    string typedValue = value.ToString();
                    property.SetValue(result, typedValue.Substring(0), null);
                }
            }

            return result;
        }
    }
}
