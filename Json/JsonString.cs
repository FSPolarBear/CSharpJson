using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json
{
    /// <summary>
    /// String item of json.
    /// <para>2024.1.11</para>
    /// <para>version 1.0.0</para>
    /// </summary>
    public class JsonString : JsonItem
    {


        // Escape characters.The key is the C# character to be escaped, and the value is the escaped json character.
        // "/" and "\" are not included.
        // This dictionary is used for encoding C# string to JsonString.
        private static readonly Dictionary<string, string> ESCAPE_CHARS_FOR_ENCODING = new Dictionary<string, string>()
        {
            ["\""] = "\\\"",
            ["\b"] = "\\b",
            ["\f"] = "\\f",
            ["\n"] = "\\n",
            ["\r"] = "\\r",
            ["\t"] = "\\t",
        };


        // Escape characters. The key is json character, and the value is decoded C# character.
        // "\u" is not included.
        // This dictionary is used for decode JsonString to C# string.
        private static readonly Dictionary<char, char> ESCAPE_CHARS_FOR_DECODING = new Dictionary<char, char>()
        {
            ['"'] = '\"',
            ['\\'] = '\\',
            ['/'] = '/',
            ['b'] = '\b',
            ['f'] = '\f',
            ['n'] = '\n',
            ['r'] = '\r',
            ['t'] = '\t',
        };

        private string value;

        public JsonString(string value)
        {
            ItemType = JsonItemType.String;
            this.value = value;
        }

        /// <summary>
        /// Parse a string to JsonString (i.e. decode a JsonString).
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static new JsonString Parse(string str)
        {
            str = str.Trim();
            // A JsonString should start with and end with a double quotation mark.
            if (str.Length < 2 || str[0] != '"' || str[str.Length - 1] != '"')
                throw new JsonFormatException(GetFormatExceptionMessage("JsonString"));
            // Remove the double quotation marks.
            str = str.Substring(1, str.Length - 2 );
            if (!str.Contains('\\')) // If no escape characters in a JsonString, it dose not need to be decoded.
                return new JsonString(str);

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != '\\') // This character is not an escape character and is written directly to the C# string.
                {
                    result.Append(str[i]);
                    continue;
                }

                i++; // str[i] is the next character of \
                char char_json = str[i];
                char char_decoded;
                if (ESCAPE_CHARS_FOR_DECODING.ContainsKey(char_json)) // Escape character except '\u'
                {
                    char_decoded = ESCAPE_CHARS_FOR_DECODING[char_json];
                    result.Append(char_decoded);
                }
                else if(char_json == 'u') // The escape character consisting of \u and four hexadecimal represents a UTF-16 encoded character.
                {
                    int unicode_int;
                    try 
                    {
                        unicode_int = Convert.ToInt32(str.Substring(i + 1, 4), 16);
                    }
                    catch(Exception ex) when (ex is ArgumentOutOfRangeException || ex is FormatException)
                    {
                        throw new JsonFormatException(GetFormatExceptionMessage("JsonString"));
                    }
                    i += 4; // str[i+4] is the last character of \u
                    // C# char use UTF-16, so convert it directly.
                    result.Append((char)unicode_int);
                }
            }

            return new JsonString(result.ToString());
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
                throw new JsonInvalidTypeException(GetInvalidTypeExceptionMessage("String", typeof(T)));
        }

        /// <summary>
        /// 
        /// <para>2024.1.11</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Encode value.
            string result = value;
            result = result.Replace("\\", "\\\\");
            foreach(KeyValuePair<string, string> kv in ESCAPE_CHARS_FOR_ENCODING)
            {
                result = result.Replace(kv.Key, kv.Value);
            }
            result = string.Format("\"{0}\"", result);
            return result;
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
