using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab6Baggage.Tests
{
    [TestClass]
    public sealed class BaggageTests
    {
        // Тестування конструктора та валідації властивостей
        [TestMethod]
        public void Constructor_WithValidArguments_CreatesBaggageObject()
        {
            string fullName = "Іваненко Іван Іванович";
            int itemsCount = 3;
            double weight = 25.5;
            var baggage = new Baggage(fullName, itemsCount, weight);
            Assert.AreEqual(fullName, baggage.FullName);
            Assert.AreEqual(itemsCount, baggage.ItemsCount);
            Assert.AreEqual(weight, baggage.TotalWeight);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Constructor_WithInvalidFullName_ThrowsArgumentException(string invalidName)
        {
            Assert.ThrowsException<ArgumentException>(() => new Baggage(invalidName, 1, 10));
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-100)]
        public void Constructor_WithInvalidItemsCount_ThrowsArgumentException(int invalidCount)
        {
            Assert.ThrowsException<ArgumentException>(() => new Baggage("Іваненко І.І.", invalidCount, 10));
        }

        [TestMethod]
        [DataRow(0.0)]
        [DataRow(-5.5)]
        [DataRow(double.NaN)]
        [DataRow(double.PositiveInfinity)]
        public void Constructor_WithInvalidWeight_ThrowsArgumentException(double invalidWeight)
        {
            Assert.ThrowsException<ArgumentException>(() => new Baggage("Іваненко І.І.", 1, invalidWeight));
        }

        // Тестування реалізації IComparable
        [TestMethod]
        public void CompareTo_WithOtherBaggage_ReturnsCorrectValue()
        {
            var b1 = new Baggage("Андрієнко А.А.", 1, 10);
            var b2 = new Baggage("Борисенко Б.Б.", 2, 20);
            var b3 = new Baggage("Андрієнко А.А.", 3, 30);
            Assert.IsTrue(b1.CompareTo(b2) < 0, "Андрієнко має бути перед Борисенком.");
            Assert.IsTrue(b2.CompareTo(b1) > 0, "Борисенко має бути після Андрієнка.");
            Assert.AreEqual(0, b1.CompareTo(b3), "Однакові ПІБ мають бути рівними.");
        }

        [TestMethod]
        public void CompareTo_WithNull_ReturnsPositiveValue()
        {
            var baggage = new Baggage("Іваненко І.І.", 1, 10);
            Assert.IsTrue(baggage.CompareTo(null) > 0, "Порівняння з null має повертати додатне значення.");
        }

        // Тестування методу ToString()
        [TestMethod]
        public void ToString_ReturnsCorrectlyFormattedString()
        {
            var baggage = new Baggage("Петренко Петро Петрович", 2, 12.5);
            string expected = "ПІБ: Петренко Петро Петрович, Кількість речей: 2, Загальна вага: 12.5 кг";
            Assert.AreEqual(expected, baggage.ToString());
        }
    }
}
