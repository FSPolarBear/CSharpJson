﻿namespace Json
{
    /// <summary>
    /// Bool item of json.
    /// <para>2024.1.3</para>
    /// <para>version 1.0.0</para>
    /// </summary>
    public class JsonBool : JsonItem
    {
        private bool value;
        public JsonBool(bool value) { 
            ItemType = JsonItemType.Bool; 
            this.value = value; 
        }

        /// <summary>
        /// Get the value of the item in the specified type.
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <typeparam name="T">JsonItem、JsonInteger、bool</typeparam>
        /// <returns>Value of the item.</returns>
        /// <exception cref="JsonInvalidTypeException">The type is invalid.</exception>
        public override T GetValue<T>()
        {
            Type type = Utils.GetActualType(typeof(T));
            if (typeof(T) == typeof(JsonItem) || typeof(T) == typeof(JsonBool))
            {
                return (T)(object)this;
            }
            else if (type == typeof(bool))
            {
                return (T)(object)value;
            }
            else
                throw new JsonInvalidTypeException(JsonExceptionMessage.GetInvalidTypeExceptionMessage("bool", type));
        }

        /// <summary>
        /// 
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString() { if (value) return "true"; else return "false"; }

        /// <summary>
        /// Parse a string to JsonBool.
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="JsonFormatException">The string cannot be parsed.</exception>
        public static new JsonBool Parse(string str)
        {
            if (str == "true")
                return new JsonBool(true);
            else if (str == "false")
                return new JsonBool(false);
            else
                throw new JsonFormatException(JsonExceptionMessage.GetFormatExceptionMessage("JsonBool"));
        }
    }
}
