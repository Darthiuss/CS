using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LR3;

namespace LR3_Tests
{
    [TestClass]
    public sealed class CarTests
    {
        // Тестування успішної ініціалізації об'єкта.
        [TestMethod]
        public void Constructor_WithValidData_InitializesCorrectly()
        {
            var car = new Car(80, 2, 7.5, EngineType.Petrol);

            Assert.AreEqual(80, car.Speed, "Швидкість має бути ініціалізована коректно.");
            Assert.AreEqual(2, car.TravelTime, "Час у дорозі має бути ініціалізований коректно.");
            Assert.AreEqual(7.5, car.FuelConsumption, "Витрата пального має бути ініціалізована коректно.");
            Assert.AreEqual(EngineType.Petrol, car.EngineType, "Тип двигуна має бути збережений.");
        }

        // Тестування конструктора з невалідною швидкістю.
        [TestMethod]
        public void Constructor_WithNegativeSpeed_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Car(-10, 2, 7.5, EngineType.Petrol));
        }

        // Тестування конструктора з нульовим часом у дорозі.
        [TestMethod]
        public void Constructor_WithZeroTravelTime_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Car(80, 0, 7.5, EngineType.Petrol));
        }

        // Тестування від'ємної витрати пального.
        [TestMethod]
        public void FuelConsumption_Negative_ThrowsArgumentException()
        {
            var car = new Car(80, 2, 7.5, EngineType.Petrol);
            Assert.ThrowsException<ArgumentException>(() => car.FuelConsumption = -1);
        }

        // Тестування невідомого типу двигуна.
        [TestMethod]
        public void Constructor_WithUnknownEngineType_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Car(80, 2, 7.5, (EngineType)99));
        }

        // Відстань = швидкість * час.
        [TestMethod]
        public void CalculateTotalDistance_ReturnsSpeedTimesTime()
        {
            var car = new Car(80, 2.5, 7.5, EngineType.Petrol);
            Assert.AreEqual(200, car.CalculateTotalDistance(), 0.0001);
        }

        // Вартість для бензинового двигуна: 200 / 100 * 8 * 50 = 800.
        [TestMethod]
        public void CalculateTripCost_Petrol_ReturnsCorrectValue()
        {
            var car = new Car(100, 2, 8, EngineType.Petrol);
            Assert.AreEqual(800, car.CalculateTripCost(50), 0.0001);
        }

        // Для дизеля діє коефіцієнт 0.9, для гібрида - 0.7.
        [TestMethod]
        public void CalculateTripCost_DieselAndHybrid_UseEngineCoefficient()
        {
            var diesel = new Car(100, 2, 8, EngineType.Diesel);
            var hybrid = new Car(100, 2, 8, EngineType.Hybrid);

            Assert.AreEqual(720, diesel.CalculateTripCost(50), 0.0001);
            Assert.AreEqual(560, hybrid.CalculateTripCost(50), 0.0001);
        }

        // Ціна пального не може бути від'ємною.
        [TestMethod]
        public void CalculateTripCost_NegativeFuelPrice_ThrowsArgumentException()
        {
            var car = new Car(100, 2, 8, EngineType.Petrol);
            Assert.ThrowsException<ArgumentException>(() => car.CalculateTripCost(-5));
        }

        // Маршрут містить тип двигуна та відстань.
        [TestMethod]
        public void GetRoute_ContainsEngineTypeAndDistance()
        {
            var car = new Car(100, 2, 8, EngineType.Diesel);
            string route = car.GetRoute();

            StringAssert.Contains(route, "Diesel");
            StringAssert.Contains(route, "приміський");
        }
    }
}