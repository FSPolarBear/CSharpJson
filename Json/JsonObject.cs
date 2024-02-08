using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json
{
    /// <summary>
    /// Json object item of json.
    /// <para>2024.2.8</para>
    /// <para>version 1.0.0</para>
    /// </summary>
    public class JsonObject : JsonItem, IDictionary<JsonString, JsonItem>
    {
        private Dictionary<JsonString, JsonItem> value;
        public JsonObject() {
            ItemType = JsonItemType.Object;
            this.value = new Dictionary<JsonString, JsonItem>();
        }
        public JsonObject(Dictionary<JsonString, JsonItem> value) {
            ItemType = JsonItemType.Object;
            this.value = value;
        }
        
        public object? this[JsonString key] { get { return this.value[key]; } set { this.value[key] = JsonItem.CreateFromValue(value); } }
        public object? this[string key] { get { return this.value[new JsonString(key)]; } set { this.value[new JsonString(key)] = JsonItem.CreateFromValue(value); } }
        


        public string[] Keys { get {
                List<string> keys = new List<string>();
                foreach (JsonString key in this.value.Keys)
                {
                    keys.Add(key.GetValue<string>());
                }
                return keys.ToArray();
            } }
        public int Count { get { return value.Count; } }

        ICollection<JsonString> IDictionary<JsonString, JsonItem>.Keys => this.value.Keys;

        public ICollection<JsonItem> Values => this.value.Values;

        public bool IsReadOnly => ((ICollection<KeyValuePair<JsonString, JsonItem>>)value).IsReadOnly;

        JsonItem IDictionary<JsonString, JsonItem>.this[JsonString key] { get => this.value[key]; set => this.value[key] = value; }

        /// <summary>
        /// Return a value indicates whether the key is included in the JsonObject.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return value.ContainsKey(new JsonString(key));
        }

        /// <summary>
        /// Get the value of the item in the specified type.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T">JsonItem, JsonObject, Dictionary<JsonString, JsonItem></typeparam>
        /// <returns></returns>
        /// <exception cref="JsonInvalidTypeException">The type is invalid.</exception>
        public override T GetValue<T>()
        {
            Type type = Utils.GetActualType(typeof(T));
            if (typeof(T) == typeof(JsonItem) || typeof(T) == typeof(JsonObject))
            {
                return (T)(object)this;
            }
            else if (type == typeof(Dictionary<JsonString, JsonItem>))
            {
                return (T)(object)value;
            }
            else
                throw new JsonInvalidTypeException(GetInvalidTypeExceptionMessage("Dictionary<JsonString, JsonItem>", type));
        }

        /// <summary>
        /// 
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            StringBuilder result = new StringBuilder();
            result.Append("{");
            bool first = true;
            foreach (KeyValuePair<JsonString, JsonItem> kv in value)
            {
                if (!first)
                    result.Append(", ");
                first = false;
                result.Append(kv.Key.ToString());
                result.Append(": ");
                result.Append(kv.Value.ToString());
            }
            result.Append("}");
            return result.ToString();
        }

        /// <summary>
        /// Return a formatted json string of the JsonObject.
        /// <para>2024.1.30</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <returns></returns>
        public string ToFormattedString()
        {
            return Format(ToString());
        }

        /// <summary>
        /// Format a json string.
        /// <para>2024.1.30</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="json">Json string</param>
        /// <returns></returns>
        public static string Format(string json)
        {
            List<string> lines = JsonToLines(json);
            int level = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("}") || lines[i].StartsWith("]"))
                    level--;
                lines[i] = new string(' ', 4 * level) + lines[i];
                if (lines[i].EndsWith("{") || lines[i].EndsWith("["))
                    level++;
            }
            return string.Join('\n', lines);
        }


        /// <summary>
        /// Parse a string to JsonObject
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="JsonFormatException">The string cannot be parsed.</exception>
        public static new JsonObject Parse(string str)
        {
            str = str.Trim();
            if (!(str.StartsWith('{') && str.EndsWith('}')))
                throw new JsonFormatException(GetFormatExceptionMessage("JsonObject"));
            if (str == "{}" || str == "{ }")
                return new JsonObject();
            int _;
            return  ParseLinesToObject(JsonToLines(str), 0, out _);
        }



        /// <summary>
        /// Get the value in the specified type by the specified key from the JsonObject.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">The key is not found.</exception>
        /// <exception cref="JsonInvalidTypeException">The type is invalid.</exception>
        public T Get<T>(JsonString key)
        {
            return value[key].GetValue<T>();
        }

        /// <summary>
        /// Get the value in the specified type by the specified key from the JsonObject.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">The key is not found.</exception>
        /// <exception cref="JsonInvalidTypeException">The type is invalid.</exception>
        public T Get<T>(string key)
        {
            return value[new JsonString(key)].GetValue<T>();
        }

        /// <summary>
        /// Get the value in the specified type by the specified key from the JsonObject. If the key is not found or the type is invalid, return a specified default value.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Get<T>(JsonString key, T defaultValue)
        {
            try
            {
                return Get<T>(key);
            }
            catch (Exception ex) 
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get the value in the specified type by the specified key from the JsonObject. If the key is not found or the type is invalid, return a specified default value.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Get<T>(string key, T defaultValue)
        {
            return Get<T>(new JsonString(key), defaultValue);
        }

        /// <summary>
        /// 
        /// <para>2024.2.8</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object? value)
        {
            this.value.Add(new JsonString(key), JsonItem.CreateFromValue(value));
        }

        /// <summary>
        /// 
        /// <para>2024.2.8</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, object?> item)
        {
            KeyValuePair<JsonString, JsonItem> _item = new KeyValuePair<JsonString, JsonItem>(new JsonString(item.Key), JsonItem.CreateFromValue(item.Value));
            ((ICollection<KeyValuePair<JsonString, JsonItem>>)value).Add(_item);
        }

        /// <summary>
        /// Remove the item at the specified key.
        /// <para>2024.2.8</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True if remove successfully, otherwise false.</returns>
        public bool Remove(string key)
        {
            return this.value.Remove(new JsonString(key));
        }

        // The following are the methods that are necessary in order to implement the IDictionary interface

        public void Add(JsonString key, JsonItem value)
        {
            this.value.Add(key, value);
        }

        public bool ContainsKey(JsonString key)
        {
            return this.value.ContainsKey(key);
        }

        public bool Remove(JsonString key)
        {
            return this.value.Remove(key);
        }

        public bool TryGetValue(JsonString key, [MaybeNullWhen(false)] out JsonItem value)
        {
            return this.value.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<JsonString, JsonItem> item)
        {
            ((ICollection<KeyValuePair<JsonString, JsonItem>>)value).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<JsonString, JsonItem>>)value).Clear();
        }

        public bool Contains(KeyValuePair<JsonString, JsonItem> item)
        {
            return ((ICollection<KeyValuePair<JsonString, JsonItem>>)value).Contains(item);
        }

        public void CopyTo(KeyValuePair<JsonString, JsonItem>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<JsonString, JsonItem>>)value).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<JsonString, JsonItem> item)
        {
            return ((ICollection<KeyValuePair<JsonString, JsonItem>>)value).Remove(item);
        }

        public IEnumerator<KeyValuePair<JsonString, JsonItem>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<JsonString, JsonItem>>)value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return value.GetEnumerator();
        }
    }
}
