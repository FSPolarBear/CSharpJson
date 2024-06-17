using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Json
{
    /// <summary>
    /// Json object item of json.
    /// </summary>
    /// 2024.5.19
    /// version 1.0.3
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



        /// <summary>
        /// Return a value indicates whether the key is contained in the JsonObject.
        /// </summary>
        /// 2024.1.4
        /// version 1.0.0
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return value.ContainsKey(new JsonString(key));
        }

        /// <summary>
        /// Get the value of the item in the specified type.
        /// </summary>
        /// 2024.1.4
        /// version 1.0.0
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
                throw new JsonInvalidTypeException(JsonExceptionMessage.GetInvalidTypeExceptionMessage("Dictionary<JsonString, JsonItem>", type));
        }

        /// <summary>
        /// Convert the JsonObject to string and append it to a StringBuilder. 
        /// </summary>
        /// 2024.3.5
        /// version 1.0.2
        /// <param name="result"></param>
        internal override void AddStringToStringBuilder(StringBuilder result)
        {
            result.Append("{");
            bool first = true;
            foreach (KeyValuePair<JsonString, JsonItem> kv in value)
            {
                if (!first)
                    result.Append(", ");
                first = false;
                kv.Key.AddStringToStringBuilder(result);
                result.Append(": ");
                kv.Value.AddStringToStringBuilder(result);
            }
            result.Append("}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// 2024.3.7
        /// version 1.0.2
        /// <returns></returns>
        public override string ToString() {
            StringBuilder result = new StringBuilder();
            AddStringToStringBuilder (result);
            return result.ToString();
        }


        /// <summary>
        /// Convert the JsonObject to formatted string and append it to a StringBuilder. 
        /// </summary>
        /// 2024.3.5
        /// version 1.0.2
        /// <param name="result"></param>
        /// <param name="level"></param>
        internal override void AddFormattedStringToStringBuilder(StringBuilder result, int level = 0)
        {
            string space = new string(' ', (level + 1) * 4);
            string space_last_line = new string(' ', level * 4);

            result.Append('{');
            if (Count == 0)
            {
                result.Append('}');
                return;
            }
            bool first = true;
            foreach (KeyValuePair<JsonString, JsonItem> kv in value)
            {
                if (!first)
                    result.Append(',');
                result.Append('\n');
                first = false;
                
                result.Append(space);
                result.Append(kv.Key.ToString());
                result.Append(": ");
                kv.Value.AddFormattedStringToStringBuilder(result, level + 1);
            }
            result.Append('\n');
            result.Append(space_last_line);
            result.Append('}');
        }
        /// <summary>
        /// 
        /// </summary>
        /// 2024.3.5
        /// version 1.0.2
        public string ToFormattedString()
        {
            StringBuilder result = new StringBuilder();
            AddFormattedStringToStringBuilder(result);
            return result.ToString();

        }



        /// <summary>
        /// Parse a string to JsonObject
        /// </summary>
        /// 2024.3.1
        /// version 1.0.2
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="JsonFormatException">The string cannot be parsed.</exception>
        public static new JsonObject Parse(string str)
        {
            if (!(str.StartsWith('{') && str.EndsWith('}')))
                throw new JsonFormatException(JsonExceptionMessage.GetFormatExceptionMessage("JsonObject"));
            int end;
            JsonObject result = JsonParser.ParseObject(str, 0, out end);
            if (end != str.Length - 1)
                throw new JsonFormatException(JsonExceptionMessage.GetFormatExceptionMessage("JsonObject"));
            return result;
        }



        /// <summary>
        /// Get the value in the specified type by the specified key from the JsonObject.
        /// </summary>
        /// 2024.1.4
        /// version 1.0.0
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
        /// </summary>
        /// 2024.1.4
        /// version 1.0.0
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
        /// </summary>
        /// 2024.1.4
        /// version 1.0.0
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
            catch (Exception) 
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get the value in the specified type by the specified key from the JsonObject. If the key is not found or the type is invalid, return a specified default value.
        /// </summary>
        /// 2024.1.4
        /// version 1.0.0
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
        /// </summary>
        /// 2024.2.8
        /// version 1.0.0
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object? value)
        {
            this.value.Add(new JsonString(key), JsonItem.CreateFromValue(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// 2024.2.8
        /// version 1.0.0
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, object?> item)
        {
            KeyValuePair<JsonString, JsonItem> _item = new KeyValuePair<JsonString, JsonItem>(new JsonString(item.Key), JsonItem.CreateFromValue(item.Value));
            ((ICollection<KeyValuePair<JsonString, JsonItem>>)value).Add(_item);
        }

        /// <summary>
        /// Remove the item at the specified key.
        /// </summary>
        /// 2024.2.8
        /// version 1.0.0
        /// <param name="key"></param>
        /// <returns>True if remove successfully, otherwise false.</returns>
        public bool Remove(string key)
        {
            return this.value.Remove(new JsonString(key));
        }

        /// <summary>
        /// Get the value by path.
        /// </summary>
        /// 2024.4.4
        /// version 1.0.3
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="JsonInvalidPathException">The path is invalid.</exception>
        /// <exception cref="JsonInvalidTypeException">The type is invalid.</exception>
        public T GetByPath<T>(string[] path)
        {
            bool isSucceed = TryGetItemByPath(path, out JsonItem? item);
            if (!isSucceed)
                throw new JsonInvalidPathException(JsonExceptionMessage.GetInvalidPathMessage());
            return item!.GetValue<T>();
        }

        /// <summary>
        /// Get the value by path.
        /// </summary>
        /// 2024.4.4
        /// version 1.0.3
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetByPath<T>(string[] path, T defaultValue)
        {
            bool isSucceed = TryGetItemByPath(path, out JsonItem? item);
            if (!isSucceed)
                return defaultValue;
            return item!.GetValue<T>(defaultValue);
        }

        /// <summary>
        /// Get the value by path.
        /// </summary>
        /// 2024.6.11
        /// version 1.0.3
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="JsonInvalidPathException">The path is invalid.</exception>
        /// <exception cref="JsonInvalidTypeException">The type is invalid.</exception>
        public T GetByPath<T>(string path, char splitCharacter = '.')
        {
            return GetByPath<T>(path.Split(splitCharacter));
        }

        /// <summary>
        /// Get the value by path.
        /// </summary>
        /// 2024.6.11
        /// version 1.0.3
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetByPath<T>(string path, T defaultValue, char splitCharacter = '.')
        {
            return GetByPath(path.Split(splitCharacter), defaultValue);
        }



        /// <summary>
        /// Convert JsonObject to a dictionary with string keys and values in a specified type. All the values should be the specified type.
        /// </summary>
        /// 2024.5.19
        /// version 1.0.3
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="JsonInvalidTypeException">The type is invalid for some values</exception>
        public Dictionary<string, T> ToDictionary<T>()
        {
            Dictionary<string, T> result = new Dictionary<string, T>();
            foreach(KeyValuePair<JsonString, JsonItem> pair in this.value)
            {
                result.Add(pair.Key.GetValue<string>(), pair.Value.GetValue<T>());
            }
            return result;
        }


        // The following are the methods that are necessary in order to implement the IDictionary interface

        ICollection<JsonString> IDictionary<JsonString, JsonItem>.Keys => this.value.Keys;

        public ICollection<JsonItem> Values => this.value.Values;

        public bool IsReadOnly => ((ICollection<KeyValuePair<JsonString, JsonItem>>)value).IsReadOnly;

        JsonItem IDictionary<JsonString, JsonItem>.this[JsonString key] { get => this.value[key]; set => this.value[key] = value; }
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
