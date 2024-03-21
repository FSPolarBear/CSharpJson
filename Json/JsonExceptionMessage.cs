namespace Json
{
    /// <summary>
    /// 
    /// <para>2024.2.26</para>
    /// <para>version 1.0.2</para>
    /// </summary>
    internal static class JsonExceptionMessage
    {
        public static string GetInvalidTypeExceptionMessage(string[] needed, Type recieved)
        {
            return string.Join(" or ", needed) + " are needed, but " + recieved + " is recieved.";
        }

        public static string GetInvalidTypeExceptionMessage(string needed, Type recieved)
        {
            return needed + " is needed, but " + recieved + " is recieved.";
        }

        public static string GetFormatExceptionMessage(string name)
        {
            return "The inputted string cannot be parsed to " + name;
        }
        public static string GetFormatExceptionMessage()
        {
            return "The inputted string cannot be parsed";
        }
    }
}
