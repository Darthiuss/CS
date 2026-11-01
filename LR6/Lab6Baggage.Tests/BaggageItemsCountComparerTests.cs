using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab6Baggage.Tests
{
    [TestClass]
    public class BaggageItemsCountComparerTests
    {
        [TestMethod]
        public void Compare_WhenCountsAreDifferent_ReturnsCorrectOrder()
        {
            var b1 = new Baggage("Андрієнко А.А.", 1, 10);
            var b2 = new Baggage("Борисенко Б.Б.", 4, 10);
            var comparer = new BaggageItemsCountComparer();
            Assert.IsTrue(comparer.Compare(b1, b2) < 0, "Запис з 1 річчю має бути перед записом з 4 речами.");
            Assert.IsTrue(comparer.Compare(b2, b1) > 0, "Запис з 4 речами має бути після запису з 1 річчю.");
        }

        [TestMethod]
        public void Compare_WhenCountsAreIdentical_ReturnsZero()
        {
            var b1 = new Baggage("Андрієнко А.А.", 3, 10);
            var b2 = new Baggage("Борисенко Б.Б.", 3, 50);
            var comparer = new BaggageItemsCountComparer();
            Assert.AreEqual(0, comparer.Compare(b1, b2), "Записи з однаковою кількістю речей мають бути рівними.");
        }

        [TestMethod]
        public void Compare_WhenFirstIsNull_ReturnsZero()
        {
            var b2 = new Baggage("Борисенко Б.Б.", 2, 10);
            var comparer = new BaggageItemsCountComparer();
            Assert.AreEqual(0, comparer.Compare(null, b2));
        }

        [TestMethod]
        public void Compare_WhenSecondIsNull_ReturnsZero()
        {
            var b1 = new Baggage("Андрієнко А.А.", 1, 10);
            var comparer = new BaggageItemsCountComparer();
            Assert.AreEqual(0, comparer.Compare(b1, null));
        }

        [TestMethod]
        public void Compare_WhenBothAreNull_ReturnsZero()
        {
            var comparer = new BaggageItemsCountComparer();
            Assert.AreEqual(0, comparer.Compare(null, null));
        }
    }
}
