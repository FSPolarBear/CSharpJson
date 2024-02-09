using Json;

namespace JsonTest
{
    /// <summary>
    /// 
    /// <para>2024.1.30</para>
    /// <para>version 1.0.0</para>
    /// </summary>
    [TestClass]
    public class JsonObjectTest
    {
        /// <summary>
        /// 
        /// <para>2024.1.11</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        [TestMethod]
        public void TestParse()
        {
            string str = "{\"a bc,ef\\\"g\": true,\"c\": {},\"d\": [[123,456],{},{\"a\": null},[1,2,3]],\"e\": [],\"g\": {\"g\": false, \"h\": true}}";
            JsonObject actural = JsonObject.Parse(str);
            Assert.IsNotNull(actural);
            Assert.IsTrue(actural.Get<bool>("a bc,ef\"g"));
            Assert.AreEqual(0, actural.Get<JsonObject>("c").Count);
            JsonArray d = actural.Get<JsonArray>("d");
            Assert.AreEqual(4, d.Count);
            JsonArray d0 = d.Get<JsonArray>(0);
            Assert.AreEqual(123, d0.Get<int>(0));
            Assert.AreEqual(456, d0.Get<int>(1));
            Assert.AreEqual(0, d.Get<JsonObject>(1).Count);
            Assert.IsNull(d.Get<JsonObject>(2).Get<object>("a"));
            Assert.AreEqual(3, d.Get<JsonArray>(3).Count);
            Console.WriteLine(actural);


        }

        [TestMethod]
        /// <summary>
        /// 
        /// <para>2024.1.30</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        public void TestFormat()
        {
            string str, actual, expected;
            str = "{\"a bc,ef\\\"g\": true,\"c\": {},\"d\": [[123,456],{},{\"a\": null},[1,2,3]],\"e\": [],\"g\": {\"g\": false, \"h\": true}}";
            expected = @"{
    ""a bc,ef\""g"": true,
    ""c"": {},
    ""d"": [
        [
            123,
            456
        ],
        {},
        {
            ""a"": null
        },
        [
            1,
            2,
            3
        ]
    ],
    ""e"": [],
    ""g"": {
        ""g"": false,
        ""h"": true
    }
}".Replace("\r\n", "\n");
            actual = JsonObject.Format(str);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test whether obj and Parse(obj.ToString()) are equal.
        /// <para>2024.1.11</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        [TestMethod]
        public void TestToStringAndParse()
        {
            string str, actual, expected;
            JsonObject obj1, obj2;

            str = "{\"a bc,ef\\\"g\": true,\"c\": {},\"d\": [[123,456],{},{\"a\": null},[1,2,3]],\"e\": [],\"g\": {\"g\": false, \"h\": true}}";
            obj1 = JsonObject.Parse(str);
            expected = obj1.ToString();
            obj2 = JsonObject.Parse(expected);
            actual = obj2.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}