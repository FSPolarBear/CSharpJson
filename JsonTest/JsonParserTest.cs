using Json;
using System.Reflection;

namespace JsonTest
{
    /// <summary>
    /// 
    /// <para>2024.3.20</para>
    /// <para>version 1.0.2</para>
    /// </summary>
    [TestClass]
    public class JsonParserTest
    {
        /// <summary>
        /// 
        /// <para>2024.3.1</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        [TestMethod]
        public void TestParseItem()
        {
            string str = "{\"int\": 123, \"float\": 1.5, \"string\": \"This is a string\",\n" +
                " \"true\": true \t,\"false\"  : false , \"null\":null,\n" +
                "\"array\": [1,2,3,\ntrue,false]," +
                "\"object\": {}}";
            int start, end;
            JsonItem item;

            // int
            start = 8;
            item = JsonParser.ParseItem(str, start, out end);
            Assert.AreEqual(10, end);
            Assert.AreEqual(123, item.GetValue<int>());
            Assert.AreEqual(JsonItemType.Integer, item.ItemType);

            // float
            start = 22; 
            item = JsonParser.ParseItem(str, start, out end);
            Assert.AreEqual(24, end);
            Assert.AreEqual(1.5, item.GetValue<double>(), 1e-5);
            Assert.AreEqual(JsonItemType.Float, item.ItemType);

            // string
            start = 37;
            item = JsonParser.ParseItem(str, start, out end);
            Assert.AreEqual(54, end);
            Assert.AreEqual("This is a string", item.GetValue<string>());

            // true
            start = 66;
            item = JsonParser.ParseItem(str, start, out end);
            Assert.AreEqual(69, end);
            Assert.IsTrue(item.GetValue<bool>());

            // false
            start = 84;
            item = JsonParser.ParseItem(str, start, out end);
            Assert.AreEqual(88, end);
            Assert.IsFalse(item.GetValue<bool>());

            // null
            start = 99;
            item = JsonParser.ParseItem(str, start, out end);
            Assert.AreEqual(102, end);
            Assert.AreEqual(JsonItemType.Null, item.ItemType);

            // array
            start = 114;
            item = JsonParser.ParseItem(str, start, out end);
            Assert.AreEqual(132, end);
            Assert.AreEqual(5, ((JsonArray)item).Count);
            Assert.AreEqual(1, ((JsonArray)item).Get<int>(0));
            Assert.AreEqual(2, ((JsonArray)item).Get<int>(1));
            Assert.AreEqual(3, ((JsonArray)item).Get<int>(2));
            Assert.IsTrue(((JsonArray)item).Get<bool>(3));
            Assert.IsFalse(((JsonArray)item).Get<bool>(4));

            // object
            start = 144;
            item = JsonParser.ParseItem(str, start, out end);
            Assert.AreEqual(145, end);
            Assert.AreEqual(0, ((JsonObject)item).Count);

            start = 0;
            item = JsonParser.ParseItem(str, start, out end);
            Assert.AreEqual(str.Length-1, end);
            Assert.AreEqual(8, ((JsonObject)item).Count);
        }

        /// <summary>
        /// 
        /// <para>2024.3.20</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        [TestMethod]
        public void TestParseNumber()
        {
            string str;
            int start=0, end;

            JsonItem item;

            str = "123456"; 
            item = JsonParser.ParseNumber(str, start, out end);
            Assert.AreEqual(str.Length-1, end);
            Assert.AreEqual(JsonItemType.Integer, item.ItemType);
            Assert.AreEqual(123456, item.GetValue<int>());

            str = "-123456.789";
            item = JsonParser.ParseNumber(str, start, out end);
            Assert.AreEqual(str.Length - 1, end);
            Assert.AreEqual(JsonItemType.Float, item.ItemType);
            Assert.AreEqual(-123456.789, item.GetValue<double>(), 1e-7);

            str = "0.001234";
            item = JsonParser.ParseNumber(str, start, out end);
            Assert.AreEqual(str.Length - 1, end);
            Assert.AreEqual(JsonItemType.Float, item.ItemType);
            Assert.AreEqual(0.001234, item.GetValue<double>(), 1e-7);

            str = "-12e+3";
            item = JsonParser.ParseNumber(str, start, out end);
            Assert.AreEqual(str.Length - 1, end);
            Assert.AreEqual(JsonItemType.Integer, item.ItemType);
            Assert.AreEqual(-12000, item.GetValue<int>());

            str = "12.34e5";
            item = JsonParser.ParseNumber(str, start, out end);
            Assert.AreEqual(str.Length - 1, end);
            Assert.AreEqual(JsonItemType.Integer, item.ItemType);
            Assert.AreEqual(1234000, item.GetValue<int>());

            str = "12.0";
            item = JsonParser.ParseNumber(str, start, out end);
            Assert.AreEqual(str.Length - 1, end);
            Assert.AreEqual(JsonItemType.Integer, item.ItemType);
            Assert.AreEqual(12, item.GetValue<int>());

            str = "-1e-3";
            item = JsonParser.ParseNumber(str, start, out end);
            Assert.AreEqual(str.Length - 1, end);
            Assert.AreEqual(JsonItemType.Float, item.ItemType);
            Assert.AreEqual(-1e-3, item.GetValue<double>(), 1e-7);

            str = "12000.00e-3";
            item = JsonParser.ParseNumber(str, start, out end);
            Assert.AreEqual(str.Length - 1, end);
            Assert.AreEqual(JsonItemType.Integer, item.ItemType);
            Assert.AreEqual(12, item.GetValue<int>());

        }
    }
}