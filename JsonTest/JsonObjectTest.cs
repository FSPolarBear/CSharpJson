using Json;

namespace JsonTest
{
    /// <summary>
    /// 
    /// </summary>
    /// 2024.6.26
    /// version 1.0.4
    [TestClass]
    public class JsonObjectTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// 2024.1.11
        /// version 1.0.0
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
        /// </summary>
        /// 2024.3.5
        /// version 1.0.2
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
            
            actual = JsonObject.Parse(str).ToFormattedString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test whether obj and Parse(obj.ToString()) are equal.
        /// </summary>
        /// 2024.1.11
        /// version 1.0.0
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

        [TestMethod]
        public void TestGetByPath()
        {
            JsonObject obj = new JsonObject()
            {
                {
                    "obj1", new JsonObject()
                    {
                        {
                            "arr1", new JsonArray()
                            {
                                true, false, new JsonObject()
                                {
                                    {
                                        "arr2",  new JsonArray()
                                        {
                                            null,  new JsonObject()
                                            {
                                                {"most_inner", "The most inner item." }
                                            }
                                        }
                                    },
                                    {"123", 456 }
                                }
                            }
                        }
                    } 
                },
                {"true", true }
            };

            object? value;
            string[] path;

            path = "true".Split('.');
            value = obj.GetByPath<bool>(path);
            Assert.AreEqual(true, value);

            path = "obj1.arr1.1".Split(".");
            value = obj.GetByPath<bool>(path);
            Assert.AreEqual(false, value);

            path = "obj1.arr1.2.123".Split(".");
            value = obj.GetByPath<int>(path);
            Assert.AreEqual(456, value);

            path = "obj1.arr1.2.arr2.0".Split(".");
            value = obj.GetByPath<object?>(path);
            Assert.IsNull(value);

            path = "obj1.arr1.2.arr2.1.most_inner".Split(".");
            value = obj.GetByPath<string>(path);
            Assert.AreEqual("The most inner item.", value);

        }

        /// <summary>
        /// 
        /// </summary>
        /// 2024.6.26
        /// version 1.0.4
        [TestMethod]
        public void TestGetDictionary()
        {
            JsonObject obj = new JsonObject() {
                {"key1", "value1" },
                {"key2", "value2" },
                {"key3", "value3" },
            };

            Dictionary<string, string> res = obj.GetValue<Dictionary<string, string>>();
            Assert.AreEqual("value1", res["key1"]);
            Assert.AreEqual("value2", res["key2"]);
            Assert.AreEqual("value3", res["key3"]);

        }
    }
}