
using NUnit.Framework;

namespace MultiKeyDictionary.Tests
{
    [TestFixture]
    public class MultiKeyDictionaryTests
    {
        [Test]
        public void CreateNewMultiKeyDictionarySuccess()
        {
            var dictionary = new MultiKeyDictionary<int, int, string>();

            Assert.NotNull(dictionary);
        }
    }
}
