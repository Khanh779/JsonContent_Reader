using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonContentReader_Lib
{
    public class JProperty : JToken
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public JProperty(string key, object value)
        {
            Value = value;
            Key = key;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "null";
        }


    }
}
