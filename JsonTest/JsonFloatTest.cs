using Json;

namespace JsonTest
{
    /// <summary>
    /// 
    /// <para>2024.3.21</para>
    /// <para>version 1.0.2</para>
    /// </summary>
    [TestClass]
    public class JsonFloatTest
    {
        /// <summary>
        /// 
        /// <para>2024.3.21</para>
        /// <para>version 1.0.2</para>
        /// </summary>
        [TestMethod]
        public void TestParse()
        {
            string str;

            JsonFloat item;

            str = "123456";
            item = JsonFloat.Parse(str);
            Assert.AreEqual(123456, item.GetValue<double>(),1e-7);



            str = "-12e+3";
            item = JsonFloat.Parse(str);
            Assert.AreEqual(-12000, item.GetValue<double>(), 1e-7);

            str = "12.34e5";
            item = JsonFloat.Parse(str);
            Assert.AreEqual(1234000, item.GetValue<double>(), 1e-7);

            str = "12.0";
            item = JsonFloat.Parse(str);
            Assert.AreEqual(12, item.GetValue<double>(), 1e-7);


            str = "12000.00e-3";
            item = JsonFloat.Parse(str);
            Assert.AreEqual(12, item.GetValue<double>(), 1e-7);

            str = "-123456.789";
            item = JsonFloat.Parse(str);
            Assert.AreEqual(-123456.789, item.GetValue<double>(), 1e-7);

            str = "0.001234";
            item = JsonFloat.Parse(str);
            Assert.AreEqual(0.001234, item.GetValue<double>(), 1e-7);

            str = "-1e-3";
            item = JsonFloat.Parse(str);
            Assert.AreEqual(-1e-3, item.GetValue<double>(), 1e-7);
        }
    }
}