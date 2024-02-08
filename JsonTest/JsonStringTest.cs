using Json;

namespace JsonTest
{
    /// <summary>
    /// 
    /// <para>2024.1.11</para>
    /// <para>version 1.0.0</para>
    /// </summary>
    [TestClass]
    public class JsonStringTest
    {
        /// <summary>
        /// 
        /// <para>2024.1.11</para>
        /// <para>version 1.0.0</para>
        /// </summary>
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
        }

        /// <summary>
        /// 
        /// <para>2024.1.11</para>
        /// <para>version 1.0.0</para>
        /// </summary>
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
        /// <para>2024.1.11</para>
        /// <para>version 1.0.0</para>
        /// </summary>
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


    }
}