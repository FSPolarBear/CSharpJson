namespace Json
{
    internal static class Utils
    {
        /// <summary>
        /// Get the actual type of nullable type. For example, int? is inputted, and int is returnd.
        /// if the input is not nullable, return the type itself.
        /// <para>2024.1.3</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetActualType(Type type)
        {   
            // int? equals Nullable<int>
            if (!type.IsGenericType) { return type; }
            if (type.GetGenericTypeDefinition()  != typeof(Nullable<>)) { return type;}
            Type[] args = type.GetGenericArguments();
            if (args.Length != 1) {  return type; }
            return args[0];
        }


        /// <summary>
        /// Check whether the given type is List<T>.
        /// <para>2024.3.7</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsList(Type type)
        {
            if (!type.IsGenericType)
                return false;
            return type.GetGenericTypeDefinition() == typeof(List<>);
        }

        /// <summary>
        /// Check whether the given type is Dictionary<TKey, TValue> and TKey is string or JsonString.
        /// <para>2024.3.7</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDictionaryWithStringKey(Type type) 
        {
            if (!type.IsGenericType)
                return false;
            if (type.GetGenericTypeDefinition() != typeof(Dictionary<,>))
                return false;
            Type[] args = type.GetGenericArguments();
            return args[0] == typeof(string) || args[0] == typeof(JsonString);
        }
    }
}
