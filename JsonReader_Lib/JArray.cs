using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonContentReader_Lib
{
    public class JArray : JToken, IEnumerable<JToken>
    {

        public JArray()
        {
            jTokens = new List<JToken>();
        }

        //JToken IList<JToken>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /* 
             {
              "name": "Project X",
              "version": "2.0",
              "description": "This is a detailed sample JSON file",
              "author": {
                "name": "John Doe",
                "email": "john.doe@example.com",
                "contact": {
                  "phone": "+1234567890",
                  "address": "123 Example Street, City, Country"
                }
              },
              "files": [
                {
                  "name": "document.docx",
                  "type": "document",
                  "size": "50KB",
                  "permissions": {
                    "read": true,
                    "write": false,
                    "execute": false
                  }
                },
                {
                  "name": "presentation.pptx",
                  "type": "presentation",
                  "size": "200KB",
                  "permissions": {
                    "read": true,
                    "write": true,
                    "execute": false
                  }
                },
                {
                  "name": "spreadsheet.xlsx",
                  "type": "spreadsheet",
                  "size": "120KB",
                  "permissions": {
                    "read": true,
                    "write": true,
                    "execute": false
                  }
                }
              ],
              "dependencies": [
                {
                  "name": "Library A",
                  "version": "1.2.3"
                },
                {
                  "name": "Library B",
                  "version": "4.5.6"
                }
              ],
              "metadata": {
                "created_at": "2024-01-01T12:00:00Z",
                "updated_at": "2024-06-01T08:00:00Z",
                "tags": ["example", "json", "sample"]
              }
            }
         
         */

        //var matches = Regex.Matches(element, @"\""([^\""]+)\"":\s*(\{[^}]*\}|\[[^\]]*\]|""[^\""]*""|\d+(\.\d+)?|true|false|null)");

        public static string Deserialize(JArray jArray, string childContent = null)
        {
            if (jArray == null) return "[]";

            string json = childContent + "[\n";
            foreach (var item in jArray.jTokens)
            {
                json += childContent + "  ";

                if (item is JObject)
                {
                    json += JObject.Deserialize((JObject)item, childContent + "  ");
                }
                else if (item is JArray)
                {
                    json += JArray.Deserialize((JArray)item, childContent + "  ");
                }
                else
                {
                    json += "\"" + item.ToString() + "\"";
                }

                json += ",\n";
            }

            if (json.EndsWith(",\n"))
            {
                json = json.Substring(0, json.Length - 2);
            }

            json += "\n" + childContent + "]";
            return json;
        }

        public static JArray Parse(string json)
        {
            var jArray = new JArray();

            var cleanedJson = json.Trim();

            // Remove outer array brackets if present
            if (cleanedJson.StartsWith("[") && cleanedJson.EndsWith("]"))
            {
                cleanedJson = cleanedJson.Substring(1, cleanedJson.Length - 2).Trim();

            }

            if (cleanedJson.StartsWith("{"))
            {
                // Pattern to match JSON objects
                var pattern = @"\{(?:[^{}]|(?<Open>\{)|(?<-Open>\}))*(?(Open)(?!))\}";
                var matches = Regex.Matches(cleanedJson, pattern, RegexOptions.Singleline);

                foreach (Match match in matches)
                {

                    var val = match.Value.Trim();
                    if (val.StartsWith("{"))
                    {
                        jArray.Add(JObject.Parse(val));
                    }
                    else if (val.StartsWith("["))
                    {
                        jArray.Add(JArray.Parse(val));
                    }
                    else
                    {
                        jArray.Add(ParsePrimitive(val));
                    }
                }
            }
            else
            {
                var elements = cleanedJson.Split(',');
                foreach (var element in elements)
                {
                    jArray.Add(new JProperty(null, element.Trim().Trim('"').TrimStart('\"').TrimEnd('\"')));
                }

            }


            return jArray;
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
                return new JProperty("", null);
            }
            else if (double.TryParse(val, out double number))
            {
                return new JProperty("", number);
            }
            throw new FormatException("Unknown value type.");
        }

        List<JToken> jTokens = new List<JToken>();

        public int Count => jTokens.Count;



        public JToken this[int index]
        {
            get
            {
                return jTokens[index];
            }
            set
            {
                jTokens[index] = value;
            }

        }


        public int IndexOf(JToken item)
        {
            return jTokens.IndexOf(item);
        }

        public void Insert(int index, JToken item)
        {
            jTokens.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            jTokens.RemoveAt(index);
        }

        public void Add(JToken item)
        {
            jTokens.Add(item);
        }

        public void Clear()
        {
            jTokens.Clear();
        }

        public bool Contains(JToken item)
        {
            return jTokens.Contains(item);
        }

        public void CopyTo(JToken[] array, int arrayIndex)
        {
            jTokens.CopyTo(array, arrayIndex);
        }

        public bool Remove(JToken item)
        {

            return jTokens.Remove(item);
        }

        public IEnumerator<JToken> GetEnumerator() => jTokens.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => jTokens.GetEnumerator();
    }
}
