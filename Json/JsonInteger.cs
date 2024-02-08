using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json
{
    /// <summary>
    /// Integer item of json.
    /// <para>2024.1.3</para>
    /// <para>version 1.0.0</para>
    /// </summary>
    public class JsonInteger : JsonItem
    {
        private long value;
        public JsonInteger(long value) { 
            ItemType = JsonItemType.Integer;
            this.value = value;
        }

        /// <summary>
        /// Get the value of the item in the specified type.
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T">JsonItem, JsonInteger, long, int, short, decimal, double, float</typeparam>
        /// <returns></returns>
        /// <exception cref="JsonInvalidTypeException">The type is invalid.</exception>
        public override T GetValue<T>()
        {
            Type type = Utils.GetActualType(typeof(T));
            if (typeof(T) == typeof(JsonItem) || typeof(T) == typeof(JsonInteger))
            {
                return (T)(object)this;
            }
            else if (type == typeof(long)){
                return (T)(object)value;
            }
            else if(type == typeof(int))
            {
                return (T)(object)(int)value;
            }
            else if(type == typeof(short)) 
            {
                return (T)(object)(short)value;
            }
            else if(type == typeof(decimal))
            {
                return (T)(object)(decimal)value;
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
                throw new JsonInvalidTypeException(GetInvalidTypeExceptionMessage(new string[] { "long", "int", "short", "decimal", "double", "float"}, type));
        }

        /// <summary>
        /// 
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return value.ToString(); }

        /// <summary>
        /// Parse a string to JsonInteger.
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="JsonFormatException">The string cannot be parsed.</exception>
        public static new JsonInteger Parse(string str)
        {
            str = str.Trim();
            long value;
            bool succeed = long.TryParse(str, out value);
            if (succeed) { return new JsonInteger(value); }
            throw new JsonFormatException(GetFormatExceptionMessage("JsonInteger"));
        }
    }
}
