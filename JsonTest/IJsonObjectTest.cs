using Json;

namespace JsonTest
{
    /// <summary>
    /// 
    /// </summary>
    /// 2024.6.19
    /// version 1.0.4
    [TestClass]
    public class IJsonObjectTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// 2024.6.19
        /// version 1.0.4
        [TestMethod]
        public void Test()
        {
            string field1 = "abc";
            TestClass1 obj1 = new TestClass2(field1);
            JsonItem jobj = JsonItem.CreateFromValue(obj1);
            Assert.IsInstanceOfType(jobj, typeof(JsonObject));
            JsonObject _jobj = (JsonObject)jobj;
            Assert.AreEqual(field1, _jobj.Get<string>("field1"));

            TestClass1 obj2 = _jobj.GetValue<TestClass1>();
            Assert.IsInstanceOfType(obj2, typeof(TestClass2));
            Assert.AreEqual(field1, obj2.field1);

            IJsonObject obj3 = _jobj.GetValue<IJsonObject>();
            Assert.IsInstanceOfType(obj3, typeof(TestClass2));

            try
            {
                TestClass3 obj4 = _jobj.GetValue<TestClass3>();
                Assert.Fail();
            }
            catch (JsonException ex) { }

            JsonObject _jobj2 = new JsonObject(new TestClass3("456"));
            IJsonObject obj5= _jobj2.GetValue<IJsonObject>();
            Assert.IsInstanceOfType(obj5, typeof(TestClass3));

            TestClass1 obj6 = _jobj2.GetValue<TestClass1>();
            Assert.IsInstanceOfType(obj5, typeof(TestClass3));

            TestClass2 obj7 = _jobj2.GetValue<TestClass2>();
            Assert.IsInstanceOfType(obj5, typeof(TestClass3));

            TestClass1 obj8 = _jobj2.GetValue<TestClass2>();
            Assert.IsInstanceOfType(obj5, typeof(TestClass3));

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// 2024.6.19
    /// version 1.0.4
    public abstract class TestClass1 : IJsonObject
    {
        public string field1;

        public abstract JsonObject ToJson();
    }

    /// <summary>
    /// 
    /// </summary>
    /// 2024.6.19
    /// version 1.0.4
    public class TestClass2 : TestClass1
    {
        public TestClass2(string filed1) { this.field1 = filed1; }

        public TestClass2(JsonObject json) { field1 = json.Get<string>("field1"); }
        public override JsonObject ToJson()
        {
            return new JsonObject { { "field1", field1 } };
        }
    }

    public class TestClass3 : TestClass2
    {
        public TestClass3(string filed1):base(filed1) { }

        public TestClass3(JsonObject json):base(json) { }

    }
}


