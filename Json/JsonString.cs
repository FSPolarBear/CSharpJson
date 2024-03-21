using System.Text;

namespace Json
{
    /// <summary>
    /// String item of json.
    /// <para>2024.3.6</para>
    /// <para>version 1.0.2</para>
    /// </summary>
    public class JsonString : JsonItem
    {

        private string value;

        public JsonString(string value)
        {
            ItemType = JsonItemType.String;
            this.value = value;
        }

        /// <summary>
        /// Parse a string to JsonString (i.e. decode a JsonString).
        /// <para>2024.3.6</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static new JsonString Parse(string str)
        {
            if (!(str.StartsWith('"') && str.EndsWith('"')))
                throw new JsonFormatException(JsonExceptionMessage.GetFormatExceptionMessage("JsonString"));
            int end;
            JsonString result = JsonParser.ParseString(str, 0, out end);
            if (end != str.Length - 1)
                throw new JsonFormatException(JsonExceptionMessage.GetFormatExceptionMessage("JsonString"));
            return result;
        }

        /// <summary>
        /// Get the value of the item in the specified type.
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T">JsonItem、JsonInteger、string</typeparam>
        /// <returns></returns>
        /// <exception cref="JsonInvalidTypeException">The type is invalid.</exception>
        public override T GetValue<T>()
        {
            if (typeof(T) == typeof(JsonItem) || typeof(T) == typeof(JsonString))
                return (T)(object)this;
            if (typeof(T) == typeof(string))
                return (T)(object)value;
            else
                throw new JsonInvalidTypeException(JsonExceptionMessage.GetInvalidTypeExceptionMessage("String", typeof(T)));
        }



        /// <summary>
        /// Convert the JsonString to string and append it to a StringBuilder. 
        /// <para>2024.3.6</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="level"></param>
        internal override void AddStringToStringBuilder(StringBuilder result)
        {
            result.Append('"');
            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case '\\': result.Append("\\\\"); break;
                    case '"': result.Append("\\\""); break;
                    case '\b': result.Append("\\b"); break;
                    case '\f': result.Append("\\f"); break;
                    case '\n': result.Append("\\n"); break;
                    case '\r': result.Append("\\r"); break;
                    case '\t': result.Append("\\t"); break;
                    default:
                        result.Append(value[i]);
                        break;
                }
            }
            result.Append('"');
        }

        /// <summary>
        /// 
        /// <para>2024.3.21</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            AddStringToStringBuilder(result);
            return result.ToString();
        }

        /// <summary>
        /// 
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            JsonString? jstring = obj as JsonString;
            if (jstring == null)
                return false;
            return jstring.value == value;
        }

        /// <summary>
        /// 
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }

}
