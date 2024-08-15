using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JsonContentReader_Lib
{
    public class JObject : JToken
    {
        private Dictionary<string, JProperty> _properties = new Dictionary<string, JProperty>();

        public JToken this[string key]
        {
            get => _properties.ContainsKey(key) ? _properties[key] : null;
            set => _properties[key] = (JProperty)value;


        }

        public JToken this[int index]
        {
            get
            {
                var array = _properties.Values.ToList().ElementAt(index);
                return array;
            }
            set
            {
                var array = _properties.Values.ToList().ElementAt(index);
                array[index] = value;
            }
        }

        JArray ConvertPropertiesToJArray(Dictionary<string, JProperty> _propers)
        {
            JArray jArray = new JArray();
            foreach (var item in _propers)
            {
                jArray.Add(item.Value);
            }
            return jArray;
        }

        public static string Deserialize(JObject jObject, string childContent = null)
        {
            if (jObject == null) return "{}";

            string json = childContent + "{\n";
            foreach (var item in jObject._properties)
            {
                var value = item.Value.Value;
                json += childContent + "  \"" + item.Key + "\": ";

                if (value is JObject)
                {
                    json += JObject.Deserialize((JObject)value, childContent + "  ");
                }
                else if (value is JArray)
                {
                    json += JArray.Deserialize((JArray)value, childContent + "  ");
                }
                else
                {
                    json += "\"" + value.ToString() + "\"";
                }

                json += ",\n";
            }

            if (json.EndsWith(",\n"))
            {
                json = json.Substring(0, json.Length - 2);
            }

            json += "\n" + childContent + "}";
            return json;
        }


        public static JObject Parse(string jsonContent)
        {
            var jObject = new JObject();

            var matches = Regex.Matches(jsonContent, @"\""([^\""]+)\"":\s*(\{[^}]*\}|\[[^\]]*\]|""[^\""]*""|\d+(\.\d+)?|true|false|null)");

            foreach (Match match in matches)
            {
                var key = match.Groups[1].Value.Trim();
                var val = match.Groups[2].Value.Trim();

                if (val.StartsWith("{"))
                {
                    jObject[key] = new JProperty(key, JObject.Parse(val));
                }
                else if (val.StartsWith("["))
                {
                    jObject[key] = new JProperty(key, JArray.Parse(val));
                }
                else
                {
                    jObject[key] = new JProperty(key, ParsePrimitive(val));
                }
            }

            return jObject;
        }

        private static JToken ParsePrimitive(string val)
        {
            if (val.StartsWith("\"") && val.EndsWith("\""))
            {
                return new JProperty("", val.Trim('"'));
            }
            else if (val.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return new JProperty("", true);
            }
            else if (val.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                return new JProperty("", false);
            }
            else if (val.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            else if (double.TryParse(val, out double number))
            {
                return new JProperty("", number);
            }
            throw new FormatException("Unknown value type.");
        }

        public IEnumerable<JProperty> Properties()
        {
            return _properties.Values;
        }

        // public IEnumerable<JProperty> Properties() => _properties.Values;

        public bool CheckJsonFormat(JObject jObject)
        {
            return jObject != null;
        }


    }

}
