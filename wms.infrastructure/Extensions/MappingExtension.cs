using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Reflection;

namespace wms.infrastructure.Extensions
{
    public static class MappingExtension
    {
        public static T ToObject<T>(this IDictionary<string, object> source) where T : new()
        {
            var someObject = new T();
            foreach (var property in someObject.GetType().GetProperties())
            {
                if (null != property && property.CanWrite)
                {
                    if (source.ContainsKey(property.Name))
                    {
                        if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(someObject, source[property.Name]?.ToString());
                        }
                        else if (property.PropertyType == typeof(Nullable<DateTime>) || property.PropertyType == typeof(DateTime))
                        {
                            if (source[property.Name] == null)
                            {
                                property.SetValue(someObject, null);
                            }
                            else if (source[property.Name] is double)
                            {
                                var d = double.Parse(source[property.Name].ToString());
                                var conv = DateTime.FromOADate(d);
                                property.SetValue(someObject, conv);
                            }
                            else
                            {
                                DateTime.TryParse(source[property.Name]?.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime value);
                                property.SetValue(someObject, value);
                            }
                        }
                        else if (property.PropertyType == typeof(Nullable<Int32>) || property.PropertyType == typeof(int))
                        {
                            int.TryParse(source[property.Name]?.ToString(), out int value);
                            property.SetValue(someObject, value);
                        }
                        else if (property.PropertyType == typeof(Nullable<decimal>) || property.PropertyType == typeof(decimal))
                        {
                            decimal.TryParse(source[property.Name]?.ToString(), out decimal value);
                            property.SetValue(someObject, value);
                        }
                        else if (property.PropertyType == typeof(Nullable<double>) || property.PropertyType == typeof(double))
                        {
                            double.TryParse(source[property.Name]?.ToString(), out double value);
                            property.SetValue(someObject, value);
                        }
                        else if (property.PropertyType == typeof(Nullable<float>) || property.PropertyType == typeof(float))
                        {
                            float.TryParse(source[property.Name]?.ToString(), out float value);
                            property.SetValue(someObject, value);
                        }
                        else if (property.PropertyType == typeof(Nullable<bool>) || property.PropertyType == typeof(bool))
                        {
                            bool.TryParse(source[property.Name]?.ToString(), out bool value);
                            property.SetValue(someObject, value);
                        }
                        else
                        {
                            var parseMethod = property.PropertyType.GetMethod("TryParse",
                                BindingFlags.Public | BindingFlags.Static, null,
                                new[] { typeof(string), property.PropertyType.MakeByRefType() }, null);
                            if (parseMethod != null)
                                if (source.ContainsKey(property.Name))
                                {
                                    var parameters = new[] { source[property.Name], null };
                                    var success = (bool)parseMethod.Invoke(null, parameters);
                                    if (success)
                                        if (property.PropertyType != typeof(string))
                                        {
                                            property.SetValue(someObject, parameters[1]);
                                            var converter = TypeDescriptor.GetConverter(property);
                                            property.SetValue(someObject, converter.ConvertFrom(source[property.Name]));
                                        }
                                        else
                                        {
                                            property.SetValue(someObject, source[property.Name]);
                                        }
                                }
                        }
                    }
                }
            }
            return someObject;
        }

        public static ExpandoObject ToObject(this IDictionary<string, object> source)
        {
            dynamic expando = new ExpandoObject();

            foreach (var dict in source)
            {
                ((IDictionary<string, object>)expando)[dict.Key] = dict.Value;
            }

            return expando;
        }
    }
}
