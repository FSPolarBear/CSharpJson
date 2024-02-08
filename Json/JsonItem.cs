using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Json
{
    /// <summary>
    /// Type of json item.
    /// <para>2024.1.3</para>
    /// <para>version 1.0.0</para>
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
    /// <para>2024.2.8</para>
    /// <para>version 1.0.0</para>
    /// </summary>
    public abstract class JsonItem
    {
        public JsonItemType ItemType { get; protected set; }

        /// <summary>
        /// Parse a string to a json item.
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="JsonFormatException">The string cannot be parsed.</exception>
        public static JsonItem Parse(string str)
        {
            str = str.Trim();
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
            else if (str.Contains('.'))
                return JsonFloat.Parse(str);
            else
                return JsonInteger.Parse(str);
        }

        /// <summary>
        /// Create a JsonItem object by the specified value. If the specified value is a JsonItem, return it without create a new object.
        /// <para>2024.1.5</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="JsonInvalidTypeException">The value cannot be convert to a JsonItem object.</exception>
        public static JsonItem CreateFromValue(object? value)
        {
            if (value == null)
                return new JsonNull();
            else if (value is JsonItem)
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
            else
                throw new JsonInvalidTypeException(GetInvalidTypeExceptionMessage(new string[] { "JsonItem", "null", "bool", "string", "long", "int", "short", "decimal", "double", "float", "List<JsonItem>", "Dictionary<JsonString, JsonItem>" }, value.GetType()));
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


        private static string BLANK_CHARS = " \n\r\t";
        /// <summary>
        /// Replace whitespace characters (space, \n, \r, \t) to spaces, and replace consecutive spaces to a space. whitespace characters surrounded by double quotation marks are ignored.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="JsonFormatException">An unclosed double quote mark exists.</exception>
        protected static string FormatBlank(string str)
        {
            str = str.Trim();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                // Ignore JsonString, i.e. string surrounded by double quotation marks.
                if (str[i] == '"')
                {
                    bool matched = false;
                    int j;
                    for (j = i + 1; j < str.Length; j++)
                    {
                        // \" is not end-of-string mark.
                        if ((str[j] == '\\'))
                            j++;
                        // " is end-of-string mark.
                        else if (str[j] == '"')
                        {
                            matched = true;
                            break;
                        }
                    }
                    // Throw an exception if the string is end but the end-of-string mark is not matched.
                    if (!matched)
                        throw new JsonFormatException(GetFormatExceptionMessage());
                    // string[j] is end-of-string mark and should be written to result.
                    result.Append(str.Substring(i, j-i+1));
                    i = j;
                }
                else if (BLANK_CHARS.Contains(str[i]))
                {
                    // For a blank character, if the previous character is not a blank character, then write a space; if not, do not write again.
                    // Since str = str.Trim(), str[0] is not a whitespace character, here i >= 1
                    if (!BLANK_CHARS.Contains(str[i - 1]))
                    {
                        result.Append(' ');
                    }
                }
                else 
                {
                    result.Append(str[i]); 
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Split a string representing a JsonObject or JsonArray into lines based on,[]{}.
        /// , -> ,\n    
        /// [ -> [\n    
        /// { -> {\n    
        /// ] -> \n]
        /// } -> \n}
        /// No \n are added into JsonString.
        /// No \n are added for empty [] or {}.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="JsonFormatException">The string is not a JsonObject or JsonArray string，or an unclosed double quote mark exists.</exception>
        protected static List<string> JsonToLines(string str)
        {
            str = FormatBlank(str);
            // JsonObject strings start with { and end with }, JsonArray strings start with [ and end with ].
            if(!(str.StartsWith('[') && str.EndsWith("]")) && !(str.StartsWith('{') && str.EndsWith('}')))
                throw new JsonFormatException(GetFormatExceptionMessage());
            List<string> result = new List<string>();
            while(str.Length > 0)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    // No \n are added into JsonString.
                    if (str[i] == '"')
                    {
                        bool matched = false; 
                        i++;
                        for (; i < str.Length; i++)
                        {
                            // \" is not end-of-string mark.
                            if ((str[i] == '\\'))
                                i++;
                            // " is end-of-string mark.
                            else if (str[i] == '"')
                            {
                                matched = true;
                                break;
                            }
                        }
                        // Throw an exception if the string is end but the end-of-string mark is not matched.
                        if (!matched)
                            throw new JsonFormatException(GetFormatExceptionMessage());
                    }
                    else if (str[i] == ',')
                    {
                        result.Add(str.Substring(0, i + 1));
                        str = str.Substring(i + 1).Trim();
                        break;
                    }
                    // { -> {\n
                    else if (str[i] == '{')
                    {
                        int j = i + 1;
                        for (; j < str.Length && BLANK_CHARS.Contains(str[j]); j++) ;
                        // No \n are added for empty {}.
                        if (j < str.Length && str[j] == '}')
                            i = j;
                        else
                        {
                            result.Add(str.Substring(0, i + 1));
                            str = str.Substring(i + 1).Trim();
                            break;
                        }
                    }
                    // [ -> [\n
                    else if (str[i] == '[')
                    {
                        int j = i + 1;
                        for (; j < str.Length && BLANK_CHARS.Contains(str[j]); j++) ;
                        // No \n are added for empty [].
                        if (j < str.Length && str[j] == ']')
                            i = j; 
                        else
                        {
                            result.Add(str.Substring(0, i + 1));
                            str = str.Substring(i + 1).Trim();
                            break;
                        }
                    }
                    // } -> \n}     ] -> \n]
                    // i > 0 is to exclude the case of the last line with only one } or ]. This case is handled in the last if.
                    else if (i > 0 && (str[i] == '}' || str[i] == ']'))
                    {
                        result.Add(str.Substring(0, i));
                        str = str.Substring(i).Trim();
                        break;
                    }

                    // If the string has been traversed, the loop ends.
                    if ( i == str.Length - 1)
                    {
                        result.Add(str);
                        str = string.Empty;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Find the key-value pair of a line. The key and the value are strings.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="line">A line from lines obtained by JsonToLines(stirng)</param>
        /// <returns></returns>
        /// <exception cref="JsonFormatException">The string is not in format "key": value</exception>
        protected static KeyValuePair<string, string> GetKeyValueStringFromLine(string line)
        {
            line = line.Trim();
            // Remove ,
            if (line[line.Length - 1] == ',')
                line = line.Substring(0, line.Length - 1);
            int pos = -1;
            // Look for : after "key"
            for (int i=0; i<line.Length; i++)
            {
                if(line[i] == '"')
                {
                    i++;
                    bool matched = false;
                    for(;i<line.Length;i++)
                    {
                        // \" is not end-of-string mark.
                        if (line[i] == '\\')
                            i++;
                        // " is end-of-string mark.
                        else if (line[i] == '"') 
                        { 
                            matched = true; break; 
                        }
                    }
                    // Throw an exception if the string is end but the end-of-string mark is not matched.
                    if (!matched)
                        throw new JsonFormatException(GetFormatExceptionMessage());
                }
                else if (line[i] == ':')
                {
                    pos = i;
                    break;
                }
            }
            // Throw an exception if : is not found.
            if (pos == -1)
                throw new JsonFormatException(GetFormatExceptionMessage());
            
            string key = line.Substring(0, pos).Trim();
            string value = line.Substring(pos+1).Trim();
            return new KeyValuePair<string, string>(key, value);
        }

        /// <summary>
        /// Find the key-value pair of a line. The key is JsonString and the value is JsonItem.
        /// This method cannot parse non-empty JsonObject or JsonArray.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="line">A line from lines obtained by JsonToLines(stirng)</param>
        /// <returns></returns>
        /// <exception cref="JsonFormatException">The string is not in format "key": value or the value cannot be parsed to JsonItem.</exception>
        protected static KeyValuePair<JsonString, JsonItem> ParseLine(string line)
        {
            KeyValuePair<string, string> kv = GetKeyValueStringFromLine(line);
            JsonString key = JsonString.Parse(kv.Key);
            JsonItem value = JsonItem.Parse(kv.Value);
            return new KeyValuePair<JsonString, JsonItem>(key, value);
        }


        /// <summary>
        /// Convert several lines of string starting from specified line to JsonObject.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="lines">A line from lines obtained by JsonToLines(stirng)/param>
        /// <param name="start">The start line</param>
        /// <exception cref="JsonFormatException">The lines cannot be parsed to JsonObject.</exception>
        /// <returns></returns>
        protected static JsonObject ParseLinesToObject(List<string> lines, int start, out int end)
        {
            JsonObject result = new JsonObject();
            if (!lines[start].Trim().EndsWith('{'))
                throw new JsonFormatException(GetFormatExceptionMessage("JsonObject"));
            {
                int i;
                for (i = start + 1; i < lines.Count; i++)
                {
                    string line = lines[i].Trim();
                    if (line.StartsWith("}"))
                    {
                        end = i;
                        return result;
                    }
                    var kv = GetKeyValueStringFromLine(line);
                    string k= kv.Key, v = kv.Value;
                    JsonString key = JsonString.Parse(k);
                    JsonItem value;
                    int _i;
                    if (v == "{")
                    {
                        value = ParseLinesToObject(lines, i, out _i);
                        i = _i;
                    }
                    else if (v =="[")
                    {
                        value = ParseLinesToArray(lines, i, out _i);
                        i = _i;
                    }
                    else
                    {
                        value = Parse(v);
                    }
                    result[key] = value;
                }

                throw new JsonFormatException(GetFormatExceptionMessage("JsonObject"));
            }
        }

        /// <summary>
        /// Convert several lines of string starting from specified line to JsonArray.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="lines">A line from lines obtained by JsonToLines(stirng)/param>
        /// <param name="start">The start line</param>
        /// <exception cref="JsonFormatException">The lines cannot be parsed to JsonArray.</exception>
        /// <returns></returns>
        protected static JsonArray ParseLinesToArray(List<string> lines, int start, out int end)
        {
            JsonArray result = new JsonArray();
            if (!lines[start].Trim().EndsWith('['))
                throw new JsonFormatException(GetFormatExceptionMessage("JsonArray"));
            {
                int i;
                for (i = start + 1; i < lines.Count; i++)
                {
                    string line = lines[i].Trim();
                    if( line.StartsWith("]"))
                    {
                        end = i;
                        return result;
                    }
                    if (line.EndsWith(","))
                        line = line.Substring(0, line.Length - 1);
                    JsonItem value;
                    int _i;
                    if (line.EndsWith("{"))
                    {
                        value = ParseLinesToObject(lines, i, out _i);
                        i = _i;
                    }
                    else if (line.EndsWith("["))
                    {
                        value = ParseLinesToArray(lines, i, out _i);
                        i = _i;
                    }
                    else
                    {
                        value = Parse(line);
                    }
                    result.Add(value);
                }

                throw new JsonFormatException(GetFormatExceptionMessage("JsonArray"));
            }
            
        }


        protected static string GetInvalidTypeExceptionMessage(string[] needed, Type recieved)
        {
            return string.Join(" or ", needed) + " are needed, but " + recieved + " is recieved.";
        }

        protected static string GetInvalidTypeExceptionMessage(string needed, Type recieved)
        {
            return needed + " is needed, but " + recieved + " is recieved.";
        }

        protected static string GetFormatExceptionMessage(string name)
        {
            return "The inputted string cannot be parsed to " + name;
        }
        protected static string GetFormatExceptionMessage()
        {
            return "The inputted string cannot be parsed";
        }
    }

 
    


    

}
