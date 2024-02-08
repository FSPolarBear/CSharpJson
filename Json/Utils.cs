using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // int? 等价于 Nullable<int>
            if (!type.IsGenericType) { return type; }
            if (type.GetGenericTypeDefinition()  != typeof(Nullable<>)) { return type;}
            Type[] args = type.GetGenericArguments();
            if (args.Length != 1) {  return type; }
            return args[0];
        }
    }
}
