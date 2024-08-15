using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonContentReader_Lib
{
    public abstract class JToken : Object, IEnumerable<JToken>
    {
        public JToken()
        {

        }

      

        public virtual JToken this[object key]
        {
            get
            {
                throw new InvalidOperationException(String.Format( "Cannot access child value on {0}.",CultureInfo.InvariantCulture, GetType()));
            }
            set
            {
                throw new InvalidOperationException(String.Format("Cannot set child value on {0}.", CultureInfo.InvariantCulture, GetType()));
            }
        }

        //public virtual JToken this[int index]
        //{
        //    get
        //    {
        //        throw new InvalidOperationException(String.Format("Cannot access child value on {0}.", CultureInfo.InvariantCulture, GetType()));
        //    }
        //    set
        //    {
        //        throw new InvalidOperationException(String.Format("Cannot set child value on {0}.", CultureInfo.InvariantCulture, GetType()));
        //    }
        //}

        public JToken SelectToken(string path)
        {
            if (path == null || path.Length == 0)
            {
                return this;
            }

            if (path[0] == '.')
            {
                path = path.Substring(1);
            }

            var paths = path.Split('.');

            JToken current = this;
            foreach (var p in paths)
            {
                if (current is JObject jObject)
                {
                    current = jObject[p];
                }
                else if (current is JArray jArray)
                {
                    // Remove brackets from the path element to get the index
                    string trimmedPath = p.Trim('[', ']');
                    if (int.TryParse(trimmedPath, out int index))
                    {
                        if (index >= 0 && index < jArray.Count)
                        {
                            current = jArray[index];
                        }
                        else
                        {
                            throw new ArgumentException("Array index out of bounds.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid array index.");
                    }
                }
                else
                {
                    return null;
                }
            }

            return current;
        }



        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<JToken>)this).GetEnumerator();
        }

        public IEnumerator<JToken> GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
