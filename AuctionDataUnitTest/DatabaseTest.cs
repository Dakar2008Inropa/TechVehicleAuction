using AuctionData.Models.Database;

namespace AuctionDataUnitTest
{
    [TestClass]
    public class DatabaseTest
    {
        [TestMethod]
        public void TestConnection()
        {
            Assert.IsTrue(Database.Instance.TestConnection());
        }
    }
}