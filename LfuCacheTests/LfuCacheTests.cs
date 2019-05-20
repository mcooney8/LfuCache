using LfuCache;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Enqueue()
        {
            // Arrange
            int expectedKey = 1;
            string expectedValue = "ABC";
            var lfuCache = new LfuCache<int, string>(1);

            // Act
            lfuCache.Add(expectedKey, expectedValue);

            // Assert
            var actualValue = lfuCache.Get(expectedKey);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCase(100)]
        [TestCase(1000)]
        public void Enqueue_Multiple(int enqueueCount)
        {
            // Arrange
            var lfuCache = new LfuCache<int, string>(enqueueCount);

            // Act
            for (int i = 0; i < enqueueCount; i++)
            {
                lfuCache.Add(i, i.ToString());
            }

            // Assert
            for (int i = 0; i < enqueueCount; i++)
            {
                var actualValue = lfuCache.Get(i);
                Assert.AreEqual(i.ToString(), actualValue);
            }
        }

        [Test]
        public void Enqueue_SingleEviction_LfuEvicted()
        {
            // Arrange
            var lfuCache = new LfuCache<int, int>(2);

            // Act
            lfuCache.Add(1, 1);
            lfuCache.Add(2, 2);
            lfuCache.Get(1);
            lfuCache.Add(3, 3);

            // Assert
            Assert.IsTrue(lfuCache.Contains(1));
            Assert.IsFalse(lfuCache.Contains(2));
            Assert.IsTrue(lfuCache.Contains(3));
        }
    }
}