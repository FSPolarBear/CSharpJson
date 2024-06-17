namespace Json
{
    /// <summary>
    /// 
    /// </summary>
    /// 2024.4.3
    /// version 1.0.3
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
            return "The inputted string cannot be parsed to " + name + ".";
        }
        public static string GetFormatExceptionMessage()
        {
            return "The inputted string cannot be parsed.";
        }

        public static string GetInvalidPathMessage()
        {
            return "The path is invalid.";
        }

    }
}
