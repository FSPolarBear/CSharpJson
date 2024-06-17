using Json;

namespace JsonTest
{
    /// <summary>
    /// 
    /// </summary>
    /// 2024.3.7
    /// version 1.0.2
    [TestClass]
    public class JsonItemTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// 2024.3.7
        /// version 1.0.2
        [TestMethod]
        public void TestCreateFromValue()
        {

            List<string> list = new List<string>() { "abc", "def"};
            JsonItem item = JsonItem.CreateFromValue(list);
            Assert.AreEqual(JsonItemType.Array, item.ItemType);
            Assert.AreEqual(2, ((JsonArray)item).Count);


            Dictionary<string, object?> dict = new Dictionary<string, object?>
            {
                {"key1", 1 },
                {"key2", 1.4 },
                {"key3", true },
                {"key4", null },
                {"key5", list },
                {"key6", new Dictionary<JsonString, int>(){
                    { new JsonString("jstr1"), 4},
                    { new JsonString("jstr2"), 6}
                    }
                }
            };
            JsonItem obj = JsonItem.CreateFromValue(dict);
            Assert.AreEqual(JsonItemType.Object, obj.ItemType);
        }
    }
}
