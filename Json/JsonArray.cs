using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json
{
    /// <summary>
    /// Array item of json
    /// <para>2024.2.8</para>
    /// <para>version 1.0.0</para>
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
                throw new JsonInvalidTypeException(GetInvalidTypeExceptionMessage(new string[] { "List<JsonItem>", "JsonItem[]"}, type));
        }
        /// <summary>
        /// 
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString() { 
            StringBuilder result = new StringBuilder();
            result.Append("[");
            bool first = true;
            for (int i = 0; i < value.Count; i++)
            {
                if (!first)
                    result.Append(", ");
                first = false;
                result.Append(value[i].ToString());
               
                
            }
            result.Append("]");
            return result.ToString();
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
            str = str.Trim();
            if (!(str.StartsWith('[') && str.EndsWith(']')))
                throw new JsonFormatException(GetFormatExceptionMessage("JsonArray"));
            if (str == "[]" || str == "[ ]")
                return new JsonArray();
            int _;
            return ParseLinesToArray(JsonToLines(str), 0, out _);
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
            catch (Exception ex)
            {
                return defualtValue;
            }
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
        /// Remove the item. 
        /// <para>2024.2.8</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if remove successfully, otherwise false.</returns>
        public bool Remove(JsonItem value) { return this.value.Remove(value); }

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
