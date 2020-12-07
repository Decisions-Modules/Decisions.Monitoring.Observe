using System;
using Decisions.Monitoring.Observe.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Decisions.Monitoring.Observe.TestSuit
{
    class TestData
    {
        public string message = "";
    };

    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            var c = Credential.GetCredential();
            var data = new TestData();
            var res = ObserveApi.PostRequest<Object, TestData>(c, "", data);
    }
    }
}
