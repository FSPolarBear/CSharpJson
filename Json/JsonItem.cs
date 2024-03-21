using System.Text;

namespace Json
{
    /// <summary>
    /// Type of json item.
    /// <para>2024.2.9</para>
    /// <para>version 1.0.1</para>
    /// </summary>
    public enum JsonItemType
    {
        String,
        Integer,
        Float,
        Bool,
        Object,
        Array,
        Null,
    }

    /// <summary>
    /// Item of json.
    /// <para>2024.3.21</para>
    /// <para>version 1.0.2</para>
    /// </summary>
    public abstract class JsonItem
    {
        public JsonItemType ItemType { get; protected set; }

        /// <summary>
        /// Parse a string to a json item.
        /// <para>2024.3.21</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="JsonFormatException">The string cannot be parsed.</exception>
        public static JsonItem Parse(string str)
        {
            if (str == "null")
                return new JsonNull();
            else if (str == "true")
                return new JsonBool(true);
            else if (str == "false")
                return new JsonBool(false);
            else if (str.Length >= 2 && str[0] == '"' && str[str.Length - 1] == '"')
                return JsonString.Parse(str);
            else if (str.Length >= 2 && str[0] == '[' && str[str.Length - 1] == ']')
                return JsonArray.Parse(str);
            else if (str.Length >= 2 && str[0] == '{' && str[str.Length - 1] == '}')
                return JsonObject.Parse(str);
            else
            {
                JsonItem result = JsonParser.ParseNumber(str, 0, out int end);
                if (end == str.Length - 1)
                    return result;
                else
                    throw new JsonFormatException(JsonExceptionMessage.GetFormatExceptionMessage());
            }
                
        }

        /// <summary>
        /// Create a JsonItem object by the specified value. If the specified value is a JsonItem, return it without create a new object.
        /// <para>2024.3.7</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="JsonInvalidTypeException">The value cannot be convert to a JsonItem object.</exception>
        public static JsonItem CreateFromValue(object? value)
        {
            if (value == null)
                return new JsonNull();
            string exceptionMessage = JsonExceptionMessage.GetInvalidTypeExceptionMessage(new string[] { "JsonItem", "null", "bool", "string", "long", "int", "short", "decimal", "double", "float", "List<JsonItem>", "Dictionary<JsonString, JsonItem>" }, value.GetType());
            if (value is JsonItem)
            {
                return (JsonItem)value;
            }
            else if (value is bool)
            {
                return new JsonBool((bool)value);
            }
            else if (value is decimal)
            {
                return new JsonFloat((decimal)value);
            }
            else if (value is double)
            {
                return new JsonFloat((decimal)(double)value);
            }
            else if (value is float)
            {
                return new JsonFloat((decimal)((float)value));
            }
            else if (value is long)
            {
                return new JsonInteger((long)value);
            }
            else if (value is int)
            {
                return new JsonInteger((long)(int)value);
            }
            else if (value is short)
            {
                return new JsonInteger((long)(short)value);
            }
            else if (value is string)
            {
                return new JsonString((string)value);
            }
            else if (value is List<JsonItem>)
            {
                return new JsonArray((List<JsonItem>)value);
            }
            else if (value is Dictionary<JsonString, JsonItem>)
            {
                return new JsonObject((Dictionary<JsonString, JsonItem>)value);
            }
            else if (value is Array) { 
                return new JsonArray((Array)value);
            }
            else if (Utils.IsList(value.GetType()))
            {
                JsonArray result = new JsonArray();
                foreach (var item in (System.Collections.IEnumerable)value)
                {
                    result.Add(item);
                }
                return result;
            }
            else if (Utils.IsDictionaryWithStringKey(value.GetType()))
            {
                JsonObject result = new JsonObject();
                System.Collections.IDictionary dic = (System.Collections.IDictionary)value;
                foreach (var key in dic.Keys) 
                { 
                    if (key is string)
                    {
                        string key_str = (string)key;
                        result[key_str] = dic[key];
                    }
                    else if (key is JsonString)
                    {
                        JsonString key_jstr = (JsonString)key;
                        result[key_jstr] = dic[key];
                    }
                    else
                        throw new JsonInvalidTypeException(exceptionMessage);
                }
                return result;
            }
            else 
                throw new JsonInvalidTypeException(exceptionMessage);
        }

        /// <summary>
        /// Get the value of the item in the specified type.
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>值</returns>
        /// <exception cref="JsonInvalidTypeException">The type is invalid.</exception>
        public abstract T GetValue<T>();

        /// <summary>
        /// Get the value of the item in the specified type. If the type is invalid, return a specified default value.
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValue<T>(T defaultValue)
        {
            try
            {
                return GetValue<T>();
            }
            catch (JsonInvalidTypeException) { return defaultValue; }    
        }

        /// <summary>
        /// Convert the json item to string and append it to a StringBuilder. 
        /// <para>2024.3.7</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        /// <param name="result"></param>
        internal virtual void AddStringToStringBuilder(StringBuilder result)
        {
            result.Append(ToString());
        }

        /// <summary>
        /// Convert the json item to string and append it to a StringBuilder. 
        /// For JsonObject and JsonArray, override this method to convert it to formatted string.
        /// JsonString is often-used in JsonObject. Therefore, for JsonString, we override this method to avoid too many string objects are generated when convert a JsonObject to string.
        /// For types except JsonObject and JsonArray, param "level" is not necessary.
        /// <para>2024.3.7</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="level"></param>
        internal virtual void AddFormattedStringToStringBuilder(StringBuilder result, int level = 0)
        {
            AddStringToStringBuilder(result);
        }

    }
}
