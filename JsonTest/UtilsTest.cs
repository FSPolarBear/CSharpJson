using Json;
using System.Net;

namespace JsonTest
{
    /// <summary>
    /// 
    /// </summary>
    /// 2024.3.7
    /// version 1.0.2
    [TestClass]
    public class UtilsTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// 2024.3.7
        /// version 1.0.2
        [TestMethod]
        public void TestGetActualType()
        {
            Type expected, actural;

            expected = typeof(int);
            actural = Utils.GetActualType(typeof(int));
            Assert.AreEqual(expected, actural);
            actural = Utils.GetActualType(typeof(int?));
            Assert.AreEqual(expected, actural);

            expected = typeof(object);
            actural = Utils.GetActualType(typeof(object));
            Assert.AreEqual(expected, actural);

            expected = typeof(string);
            actural = Utils.GetActualType(typeof(string));
            Assert.AreEqual(expected, actural);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 2024.3.7
        /// version 1.0.2
        [TestMethod]
        public void TestIsList() 
        { 
            bool actural;

            actural = Utils.IsList(typeof(List<int?>));
            Assert.IsTrue(actural);
            actural = Utils.IsList(typeof(List<object>));
            Assert.IsTrue(actural);
            actural = Utils.IsList(typeof(List<JsonItem>));
            Assert.IsTrue(actural);
            actural = Utils.IsList(typeof(object[]));
            Assert.IsFalse(actural);
            actural = Utils.IsList(typeof(object));
            Assert.IsFalse(actural);
            actural = Utils.IsList(typeof(string));
            Assert.IsFalse(actural);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 2024.3.7
        /// version 1.0.2
        [TestMethod]
        public void TestIsDictionaryWithStringKey()
        {
            bool actural;

            actural = Utils.IsDictionaryWithStringKey(typeof(Dictionary<string, object>));
            Assert.IsTrue(actural);
            actural = Utils.IsDictionaryWithStringKey(typeof(Dictionary<String, object>));
            Assert.IsTrue(actural);
            actural = Utils.IsDictionaryWithStringKey(typeof(Dictionary<string, int>));
            Assert.IsTrue(actural);
            actural = Utils.IsDictionaryWithStringKey(typeof(Dictionary<JsonString, object>));
            Assert.IsTrue(actural);
            actural = Utils.IsDictionaryWithStringKey(typeof(Dictionary<object, object>));
            Assert.IsFalse(actural);
            actural = Utils.IsDictionaryWithStringKey(typeof(object));
            Assert.IsFalse(actural);
        }
    }
}