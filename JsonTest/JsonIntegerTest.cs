using Json;

namespace JsonTest
{
    /// <summary>
    /// 
    /// <para>2024.1.4</para>
    /// <para>version 1.0.0</para>
    /// </summary>
    [TestClass]
    public class JsonIntegerTest
    {
        /// <summary>
        /// 
        /// <para>2024.1.4</para>
        /// <para>version 1.0.0</para>
        /// </summary>
        [TestMethod]
        public void TestGetValue()
        {
            JsonInteger integer = new JsonInteger(100);
            long? a = integer.GetValue<long?>();
            Assert.AreEqual(100L, a);
            int b = integer.GetValue<int>();
            Assert.AreEqual(100, b);
            short c = integer.GetValue<short>();
            Assert.AreEqual(100, c);
            decimal d = integer.GetValue<decimal>();
            Assert.AreEqual(100M, d, 1e-5M);
            double e = integer.GetValue<double>();
            Assert.AreEqual(100, e, 1e-5);
            float f = integer.GetValue<float>();
            Assert.AreEqual(100f, f, 1e-5);
            try
            {
                string g = integer.GetValue<string>();
                Assert.Fail();
            }catch (Exception ex) { }

            JsonItem h = integer.GetValue<JsonItem>();
            Assert.AreSame(integer, h);
            JsonInteger i = integer.GetValue<JsonInteger>();
            Assert.AreSame(integer, i);
        }

        [TestMethod]
        public void TestParse() 
        {

        }
    }
}