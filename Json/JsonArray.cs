using System.Collections;
using System.Text;

namespace Json
{
    /// <summary>
    /// Array item of json
    /// <para>2024.3.5</para>
    /// <para>version 1.0.2</para>
    /// </summary>
    public class JsonArray : JsonItem, IEnumerable<JsonItem>
    {
        private List<JsonItem> value;
        public JsonArray() {
            ItemType = JsonItemType.Array;
            this.value = new List<JsonItem>() ; 
        }
        public JsonArray(List<JsonItem> value) {
            ItemType = JsonItemType.Array;
            this.value = value;
        }

        public JsonArray(Array value)
        {
            ItemType = JsonItemType.Array;
            this.value = new List<JsonItem>();
            foreach (object? item in value) { this.value.Add(CreateFromValue(item)); }
        }

        public object? this[int index] { get { return this.value[index]; } set { this.value[index] = JsonItem.CreateFromValue(value); } }
        public int Count { get { return this.value.Count; } }


        /// <summary>
        /// Get the value of the item in the specified type.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T">JsonItem, JsonArray, List<JsonItem>, JsonItem[]</typeparam>
        /// <returns>Value of the item.</returns>
        /// <exception cref="JsonInvalidTypeException">The type is invalid.</exception>
        public override T GetValue<T>()
        {
            Type type = Utils.GetActualType(typeof(T));
            if (typeof(T) == typeof(JsonItem) || typeof(T) == typeof(JsonArray))
            {
                return (T)(object)this;
            }
            else if (type == typeof(List<JsonItem>)){
                return (T)(object)value;
            }
            else if(type == typeof(JsonItem[]))
            {
                return (T)(object)value.ToArray();
            }
            else
                throw new JsonInvalidTypeException(JsonExceptionMessage.GetInvalidTypeExceptionMessage(new string[] { "List<JsonItem>", "JsonItem[]"}, type));
        }

        /// <summary>
        /// Parse a string to JsonArray.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="JsonFormatException">The string cannot be parsed.</exception>
        public static new JsonArray Parse(string str)
        {
            if (!(str.StartsWith('[') && str.EndsWith(']')))
                throw new JsonFormatException(JsonExceptionMessage.GetFormatExceptionMessage("JsonArray"));
            int end;
            JsonArray result = JsonParser.ParseArray(str, 0, out end);
            if (end != str.Length - 1)
                throw new JsonFormatException(JsonExceptionMessage.GetFormatExceptionMessage("JsonArray"));
            return result;
        }

        /// <summary>
        /// Get the value in the specified type at the specified index from the JsonArray.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">The index is out of range.</exception>
        /// <exception cref="JsonInvalidTypeException">The type is invalid.</exception>
        public T Get<T>(int index)
        {
            return value[index].GetValue<T>();
        }

        /// <summary>
        /// Get the value in the specified type at the specified index from the JsonArray. If the index is out of range or the type is invalid, return a specified default value.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns>Value of the item.</returns>
        public T Get<T>(int index, T defualtValue)
        {
            try
            {
                return Get<T>(index);
            }
            catch (Exception)
            {
                return defualtValue;
            }
        }

        /// <summary>
        /// Convert the JsonArray to string and append it to a StringBuilder. 
        /// <para>2024.3.7</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="level"></param>
        internal override void AddStringToStringBuilder(StringBuilder result)
        {
            result.Append("[");
            bool first = true;
            for (int i = 0; i < value.Count; i++)
            {
                if (!first)
                    result.Append(", ");
                first = false;
                //result.Append(value[i].ToString());
                value[i].AddStringToStringBuilder(result);


            }
            result.Append("]");
        }

        /// <summary>
        /// 
        /// <para>2024.3.7</para>
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
        /// Convert the JsonArray to formatted string and append it to a StringBuilder. 
        /// <para>2024.3.5</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="level"></param>
        internal override void AddFormattedStringToStringBuilder(StringBuilder result, int level = 0)
        {
            string space = new string(' ', (level+1) * 4);
            string space_last_line = new string(' ', level * 4);
            result.Append('[');
            if (Count == 0)
            {
                result.Append(']');
                return;
            }
            result.Append('\n');
            result.Append(space);
            value[0].AddFormattedStringToStringBuilder(result, level + 1);
            for(int i = 1; i < Count; i++)
            {
                result.Append(",\n");
                result.Append(space);
                value[i].AddFormattedStringToStringBuilder(result, level + 1);
            }
            result.Append('\n');
            result.Append(space_last_line);
            result.Append(']');
        }
        /// <summary>
        /// 
        /// <para>2024.3.5</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        public string ToFormattedString()
        {
            StringBuilder result = new StringBuilder();
            AddFormattedStringToStringBuilder(result);
            return result.ToString();
            
        }

        /// <summary>
        /// Add a value to the JsonArray.
        /// <para>2024.1.11</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="JsonInvalidTypeException">The value cannot be convert to a JsonItem object.</exception>
        public void Add(object? value)
        {
            this.value.Add(JsonItem.CreateFromValue(value));
        }

        /// <summary>
        /// Remove the item at the specified index.
        /// <para>2024.2.8</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index) { this.value.RemoveAt(index); }

        /// <summary>
        /// Insert an item into the JsonArray at the specified index.
        /// <para>2024.3.7</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        /// <param name="index"></param>
        public void Insert(int index, object? item)
        {
            this.value.Insert(index, CreateFromValue(item));
        }

        public IEnumerator<JsonItem> GetEnumerator()
        {
            return value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return value.GetEnumerator();
        }


    }
}
