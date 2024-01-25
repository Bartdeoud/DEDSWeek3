using Game;

namespace Game_Tests
{
    [TestClass]
    public class Stapel_Tests
    {
        [TestMethod]
        public void TestAddingToStapel()
        {
            Stapel<string> stapel = new Stapel<string>();

            stapel.AddToStapel("Test1");
            stapel.AddToStapel("Test2");

            Assert.AreEqual(stapel.CurrentObject.MainObject, stapel.TakeFromStapel());
        }

        [TestMethod]
        public void TestTakeFromStapel() 
        {
            Stapel<string> stapel = new Stapel<string>();

            stapel.AddToStapel("Test1");
            stapel.AddToStapel("Test2");
            stapel.AddToStapel("Test3");

            stapel.TakeFromStapel();

            Assert.AreEqual("Test2", stapel.TakeFromStapel());
        }

        [TestMethod]
        public void TestResponseEmptyStapel()
        {
            Stapel<string> stapel = new Stapel<string>();

            Assert.ThrowsException<NullReferenceException>(() => stapel.TakeFromStapel());
        }
    }
}