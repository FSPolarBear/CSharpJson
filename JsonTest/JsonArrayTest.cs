using Json;

namespace JsonTest;

/// <summary>
/// 
/// </summary>
/// 2024.6.26
/// version 1.0.4
[TestClass]
public class JsonArrayTest
{

    /// <summary>
    /// 
    /// </summary>
    /// 2024.6.26
    /// version 1.0.4
    [TestMethod]
    public void TestGetListAndArray()
    {
        JsonArray arr = new JsonArray() { 0, 1, 2};
        List<int> res = arr.GetValue<List<int>>();
        Assert.AreEqual(3, res.Count);

        int[] res2 = arr.GetValue<int[]>();
        Assert.AreEqual(3, res2.Length);

        JsonArray arr2 = new JsonArray() { 0, 1, true };
        try
        {
            arr2.GetValue<List<int>>();
            Assert.Fail();
        }catch(JsonInvalidTypeException ex) {
            
        }
    }
}
