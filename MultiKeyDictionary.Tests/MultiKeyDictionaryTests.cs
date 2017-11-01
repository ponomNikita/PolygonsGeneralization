using NUnit.Framework;

namespace MultiKeyDictionary.Tests
{
    internal class TestEntity : IHasKey<int>, IHasKey<string>
    {
        public TestEntity(int value1, string value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public TestEntity()
        {}

        public int Value1 { get; set; }
        public string Value2 { get; set; }
        int IHasKey<int>.GetKey()
        {
            return Value1;
        }

        string IHasKey<string>.GetKey()
        {
            return Value2;
        }
    }

    [TestFixture]
    public class MultiKeyDictionaryTests
    {
        [Test]
        public void CreateNewMultiKeyDictionarySuccessTest()
        {
            var dictionary = new MultiKeyDictionary<int, string, TestEntity>();

            Assert.NotNull(dictionary);
            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void AddEntityTest()
        {
            var dictionary = new MultiKeyDictionary<int, string, TestEntity>();

            dictionary.Add(new TestEntity(1, "first"));

            Assert.AreEqual(1, dictionary.Count);
        }

        [Test]
        public void AddSomeEntitiesTest()
        {
            var dictionary = new MultiKeyDictionary<int, string, TestEntity>();

            dictionary.Add(new TestEntity(1, "first"));
            dictionary.Add(new TestEntity(2, "second"));
            dictionary.Add(new TestEntity(3, "third"));

            Assert.AreEqual(3, dictionary.Count);
        }

        [Test]
        public void FindByKey1Test()
        {
            var dictionary = new MultiKeyDictionary<int, string, TestEntity>();

            dictionary.Add(new TestEntity(1, "first"));
            dictionary.Add(new TestEntity(2, "second"));
            dictionary.Add(new TestEntity(3, "third"));

            var entity = dictionary.FindByKey1(2);

            Assert.NotNull(entity);
            Assert.AreEqual(2, entity.Value1);
            Assert.AreEqual("second", entity.Value2);
        }

        [Test]
        public void FindByKey2Test()
        {
            var dictionary = new MultiKeyDictionary<int, string, TestEntity>();

            dictionary.Add(new TestEntity(1, "first"));
            dictionary.Add(new TestEntity(2, "second"));
            dictionary.Add(new TestEntity(3, "third"));

            var entity = dictionary.FindByKey2("second");

            Assert.NotNull(entity);
            Assert.AreEqual(2, entity.Value1);
            Assert.AreEqual("second", entity.Value2);
        }

        [Test]
        public void ClearTest()
        {
            var dictionary = new MultiKeyDictionary<int, string, TestEntity>();

            dictionary.Add(new TestEntity(1, "first"));
            dictionary.Add(new TestEntity(2, "second"));
            dictionary.Add(new TestEntity(3, "third"));

            dictionary.Clear();
            
            Assert.AreEqual(0, dictionary.Count);
            Assert.Null(dictionary.FindByKey1(1));
            Assert.Null(dictionary.FindByKey1(2));
            Assert.Null(dictionary.FindByKey1(3));
            Assert.Null(dictionary.FindByKey2("first"));
            Assert.Null(dictionary.FindByKey2("second"));
            Assert.Null(dictionary.FindByKey2("third"));
        }

        [Test]
        public void RemoveEntityTest()
        {
            var dictionary = new MultiKeyDictionary<int, string, TestEntity>();

            var first = new TestEntity(1, "first");
            var second = new TestEntity(2, "second");
            var third = new TestEntity(3, "third");

            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);

            dictionary.Remove(second);

            var findRemovedByKey1 = dictionary.FindByKey1(2);
            var findRemovedByKey2 = dictionary.FindByKey2("second");

            Assert.AreEqual(2, dictionary.Count);
            Assert.Null(findRemovedByKey1);
            Assert.Null(findRemovedByKey2);
        }

        [Test]
        public void ToListTest()
        {
            var dictionary = new MultiKeyDictionary<int, string, TestEntity>();

            dictionary.Add(new TestEntity(1, "first"));
            dictionary.Add(new TestEntity(2, "second"));
            dictionary.Add(new TestEntity(3, "third"));

            var list = dictionary.ToList();

            Assert.NotNull(list);
            Assert.AreEqual(3, list.Count);

            foreach (var entity in list)
            {
                Assert.True(dictionary.Contains(entity));
            }
        }

        [Test]
        public void GetEnumeratorTest()
        {
            var dictionary = new MultiKeyDictionary<int, string, TestEntity>();

            dictionary.Add(new TestEntity(1, "first"));
            dictionary.Add(new TestEntity(2, "second"));
            dictionary.Add(new TestEntity(3, "third"));

            foreach (TestEntity entity in dictionary)
            {
                Assert.True(dictionary.Contains(entity));
            }
        }
    }
}
