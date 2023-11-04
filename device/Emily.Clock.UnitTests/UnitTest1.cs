using nanoFramework.TestFramework;
using System;
using nanoFramework.Json;

namespace Emily.Clock.UnitTests
{
    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var expected = new TestClass();
            var serialized = JsonConvert.SerializeObject(expected);
            
            Console.WriteLine(serialized);

            var deserialized = (TestClass) JsonConvert.DeserializeObject(serialized, typeof(TestClass));

            Assert.IsTrue(true);
            Assert.AreEqual(expected.Property.TotalMilliseconds, deserialized.Property.TotalMilliseconds);
        }
    }

    public class TestClass
    {
        public TimeSpan Property { get; set; } = new TimeSpan(1, 2, 3);
    }
}
