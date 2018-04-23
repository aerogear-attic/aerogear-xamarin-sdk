using System;
using NUnit.Framework;

namespace AeroGear.Mobile.Core.Utils
{
    [TestFixture]
    public class SanityCheckTests
    {
        [Test]
        public void testNonNull()
        {
            try
            {
                SanityCheck.NonNull((String)null, "test-param");
                Assert.Fail("null value has not been detected");
            }
            catch (ArgumentNullException ane)
            {
                Assert.AreEqual(String.Format("Value cannot be null.{0}Parameter name: test-param", Environment.NewLine), ane.Message);
            }
        }

        [Test]
        public void testNonEmpty()
        {
            try
            {
                SanityCheck.NonEmpty("     ", "empty-string");
                Assert.Fail("empty value has not been detected");
            }
            catch (ArgumentException ae)
            {
                Assert.AreEqual("'empty-string' can't be empty or null", ae.Message);
            }
        }

        [Test]
        public void testNonEmptyNoTrim()
        {
            SanityCheck.NonEmpty("     ", "empty-string", false);
        }

        [Test]
        public void testNonEmptyWithCustomMessage()
        {
            try
            {
                SanityCheck.NonEmpty("     ",
                                     "Parameter '{0}' must be valorised and only spaces are not accepted",
                                     "testParam");
                Assert.Fail("empty value has not been detected");
            }
            catch (ArgumentException ae)
            {
                Assert.AreEqual(
                    "Parameter 'testParam' must be valorised and only spaces are not accepted",
                    ae.Message);
            }
        }
    }
}
