using Bookzilla.Admin.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void TestMethod1()
        {
            CollectionAPIClient client = new CollectionAPIClient();
            var result = await client.GetCollections();
            Assert.IsNotNull(result);
        }
    }
}
