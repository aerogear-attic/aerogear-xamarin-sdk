using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AeroGear.Mobile.Core.Tests.AeroGear.Mobile
{
    [TestFixture]
    public class AsyncExperiments
    {
        [Test]
        public async Task TestAsync()
        {
            var result = await One() + await Two();
            Assert.AreEqual(3, result);
        }


        public async Task<int> One() {
            await Task.Delay(200);
            return 1;
        }

        public async Task<int> Two()
        {
            await Task.Delay(400);
            return 2;
        }
    }
}
