using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json
{
    /// <summary>
    /// Non-integer number item of json.
    /// <para>2024.1.3</para>
    /// <para>version 1.0.0</para>
    /// </summary>
    public class JsonFloat : JsonItem
    {
        private decimal value;
        public JsonFloat(decimal value) {
            ItemType = JsonItemType.Float;
            this.value = value; 
        }

        /// <summary>
        /// Get the value of the item in the specified type.
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T">JsonItem, JsonFloat, decimal, double, float</typeparam>
        /// <returns></returns>
        /// <exception cref="JsonInvalidTypeException">The type is invalid.</exception>
        public override T GetValue<T>()
        {
            Type type = Utils.GetActualType(typeof(T));
            if (typeof(T) == typeof(JsonItem) || typeof(T) == typeof(JsonFloat))
            {
                return (T)(object)this;
            }
            else if (type == typeof(decimal))
            {
                return (T)(object)value;
            }
            else if (type == typeof(double))
            {
                return (T)(object)(double)value;
            }
            else if (type == typeof(float))
            {
                return (T)(object)(float)value;
            }
            else
                throw new JsonInvalidTypeException(GetInvalidTypeExceptionMessage(new string[] { "decimal", "double", "float" }, type));
        }

        public override string ToString() { return value.ToString(); }

        /// <summary>
        /// Parse a string to JsonFloat.
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="JsonFormatException">The string cannot be parsed.</exception>
        public static new JsonFloat Parse(string str)
        {
            str = str.Trim();
            decimal value;
            bool succeed = decimal.TryParse(str, out value);
            if (succeed) { return new JsonFloat(value); }
            throw new JsonFormatException(GetFormatExceptionMessage("JsonFloat"));
        }
    }
}
