using Json;

namespace JsonTest
{
    /// <summary>
    /// 
    /// </summary>
    /// 2024.5.19
    /// version 1.0.3
    [TestClass]
    public class JsonStringTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// 2024.2.26
        /// version 1.0.2
        [TestMethod]
        public void TestParse()
        {
            string str, actual, expected;
            JsonString obj;

            str = "\"abcde\"";
            obj = JsonString.Parse(str);
            actual = obj.GetValue<string>();
            expected = "abcde";
            Assert.AreEqual(expected, actual);

            str = "\"\"";
            obj = JsonString.Parse(str);
            actual = obj.GetValue<string>();
            expected = "";
            Assert.AreEqual(expected, actual);

            str = "\"a\\nb\\fc\\td\\\"e\\/\"";
            obj = JsonString.Parse(str);
            actual = obj.GetValue<string>();
            expected = "a\nb\fc\td\"e/";
            Assert.AreEqual(expected, actual);

            str = "\"abcd\\u9999efg\"";
            obj = JsonString.Parse(str);
            actual = obj.GetValue<string>();
            expected = "abcd\x9999efg";
            Assert.AreEqual(expected, actual);

            try
            {
                str = "abcde";
                obj = JsonString.Parse(str);
                Assert.Fail();
            }catch (JsonFormatException) { }

            str = "\"ab\"cd\"";
            try
            {
                obj = JsonString.Parse(str);
                Assert.Fail();
            }
            catch (JsonFormatException) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 2024.1.11
        /// version 1.0.0
        [TestMethod]
        public void TestToString()
        {
            string str, actual, expected;
            JsonString obj;

            str = "abcde";
            obj = new JsonString(str);
            actual = obj.ToString();
            expected = "\"abcde\"";
            Assert.AreEqual(expected, actual);

            str = "";
            obj = new JsonString(str);
            actual = obj.ToString();
            expected = "\"\"";
            Assert.AreEqual(expected, actual);

            str = "ab\nc\fd\te\"f/g\\h";
            obj = new JsonString(str);
            actual = obj.ToString();
            expected = "\"ab\\nc\\fd\\te\\\"f/g\\\\h\"";
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
            string str;
            JsonString actual, expected;
            
            str = "abcde";
            expected = new JsonString(str);
            actual = JsonString.Parse(expected.ToString());
            Assert.AreEqual(expected, actual);

            str = "";
            expected = new JsonString(str);
            actual = JsonString.Parse(expected.ToString());
            Assert.AreEqual(expected, actual);

            str = "ab\nc\fd\te\"f/g\\h";
            expected = new JsonString(str);
            actual = JsonString.Parse(expected.ToString());
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// Test generating a JsonString by a char or get a char value from JsonString.
        /// </summary>
        /// 2024.5.19
        /// version 1.0.3
        [TestMethod]
        public void TestChar()
        {
 
            JsonItem jstr1 = JsonItem.CreateFromValue('a');
            Assert.IsTrue(jstr1 is JsonString);
            Assert.AreEqual('a', jstr1.GetValue<char>());
            Assert.AreEqual("a", jstr1.GetValue<string>());

            JsonItem jstr2 = JsonItem.CreateFromValue("a");
            Assert.IsTrue(jstr2 is JsonString);
            Assert.AreEqual('a', jstr2.GetValue<char>());
            Assert.AreEqual("a", jstr2.GetValue<string>());

            JsonItem jstr3 = JsonItem.CreateFromValue("ab");
            try
            {
                jstr3.GetValue<char>();
                Assert.Fail();
            }catch(JsonInvalidTypeException) { }
        }

    }
}