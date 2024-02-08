using Json;

namespace JsonTest
{

    /// <summary>
    /// Convert the protected methods in the JsonItem class to public for testing.
    /// <para>2024.1.4</para>
    /// <para>version 1.0.0</para>
    /// </summary>
    public class _JsonItem : JsonItem
    {
        public override T GetValue<T>()
        {
            throw new NotImplementedException();
        }

        public static new string FormatBlank(string str)
        {
            return JsonItem.FormatBlank(str);
        }
        public static new List<string> JsonToLines(string str)
        {
            return JsonItem.JsonToLines(str);
        }

        public static new KeyValuePair<string, string> GetKeyValueStringFromLine(string line)
        {
            return JsonItem.GetKeyValueStringFromLine(line);
        }

        public static new KeyValuePair<JsonString, JsonItem> ParseLine(string line)
        {
            return JsonItem.ParseLine(line);
        }

    }

    /// <summary>
    ///   
    /// <para>2024.1.10</para>
    /// <para>version 1.0.0</para>
    /// </summary>
    [TestClass]
    public class JsonItemTest
    {

        /// <summary>
        /// 
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        [TestMethod]
        public void TestFormatBlank()
        {
            string str = "{\"a bc,ef\\\"g\":true\n\n\n,\"c\":{\n  \t   \r},\"d\":[[123,456],{},{\"a\":true  \t },[1,2,3]] \n\r,\"e\":[],\"g\" : {\"g\":false}}";
            string actual = _JsonItem.FormatBlank(str);
            string expected = "{\"a bc,ef\\\"g\":true ,\"c\":{ },\"d\":[[123,456],{},{\"a\":true },[1,2,3]] ,\"e\":[],\"g\" : {\"g\":false}}";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// 
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        [TestMethod]
        public void TestJsonToLines()
        {
            string str = "{\"a bc,ef\\\"g\":true\n\n\n,\"c\":{\n     \r},\"d\":[[123,456],{},{\"a\":true   },[1,2,3]] \n\r,\"e\":[],\"g\" : {\"g\":false}}";
            List<string> actual = _JsonItem.JsonToLines(str);
            List<string> expected = new List<string>()
            {
                "{", "\"a bc,ef\\\"g\":true ,", "\"c\":{ },", "\"d\":[", "[", "123,", "456", "],", "{},", "{", "\"a\":true ", "},", "[", "1,", "2,", "3",
                "]", "] ,", "\"e\":[],", "\"g\" : {", "\"g\":false", "}", "}"
            };
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++) { Assert.AreEqual(expected[i], actual[i]); }


            actual = _JsonItem.JsonToLines("{}");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("{}", actual[0]);

        }

        /// <summary>
        /// 
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        [TestMethod]
        public void TestGetKeyValueStringFromLine()
        {
            string str;
            KeyValuePair<string, string> actural;
            KeyValuePair<string, string> expected;

            str = "\"abc\":123,";
            actural = new KeyValuePair<string, string>("\"abc\"", "123");
            expected = _JsonItem.GetKeyValueStringFromLine(str);
            Assert.AreEqual(expected.Key, actural.Key);
            Assert.AreEqual(expected.Value, actural.Value);

            str = "\"ab:c\":123,";
            actural = new KeyValuePair<string, string>("\"ab:c\"", "123");
            expected = _JsonItem.GetKeyValueStringFromLine(str);
            Assert.AreEqual(expected.Key, actural.Key);
            Assert.AreEqual(expected.Value, actural.Value);

            str = "\"ab:c\":[";
            actural = new KeyValuePair<string, string>("\"ab:c\"", "[");
            expected = _JsonItem.GetKeyValueStringFromLine(str);
            Assert.AreEqual(expected.Key, actural.Key);
            Assert.AreEqual(expected.Value, actural.Value);

            str = "\"ab:c\":{},";
            actural = new KeyValuePair<string, string>("\"ab:c\"", "{}");
            expected = _JsonItem.GetKeyValueStringFromLine(str);
            Assert.AreEqual(expected.Key, actural.Key);
            Assert.AreEqual(expected.Value, actural.Value);
        }

        /// <summary>
        /// 
        /// <para>2024.1.10</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        public void TestParseLine()
        {
            string str;
            KeyValuePair<JsonString, JsonItem> actural;
            string key;
            object? value;

            str = "\"a\\tbc\"  :  123 ,";
            actural = _JsonItem.ParseLine(str);
            key = "a\tbc";
            value = 123;
            Assert.AreEqual(key, actural.Key.GetValue<string>());
            Assert.AreEqual(value, actural.Value.GetValue<int>());

            str = "\"a\\tbc\"  :  \"123\" ,";
            actural = _JsonItem.ParseLine(str);
            key = "a\tbc";
            value = "123";
            Assert.AreEqual(key, actural.Key.GetValue<string>());
            Assert.AreEqual(value, actural.Value.GetValue<string>());

            str = "\"a\\tbc\"  :  {} ,";
            actural = _JsonItem.ParseLine(str);
            key = "a\tbc";
            Assert.AreEqual(key, actural.Key.GetValue<string>());
            Assert.AreEqual(0, actural.Value.GetValue<JsonObject>().Count);


            str = "\"a\\tbc\"  :  [ ] ,";
            actural = _JsonItem.ParseLine(str);
            key = "a\tbc";
            Assert.AreEqual(key, actural.Key.GetValue<string>());
            Assert.AreEqual(0, actural.Value.GetValue<JsonArray>().Count);

        }


    }
}