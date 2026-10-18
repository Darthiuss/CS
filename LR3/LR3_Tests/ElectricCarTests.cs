using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace LR3.Tests
{
    [TestClass]
    public sealed class ElectricCarTests
    {
        // Тестування успішної ініціалізації об'єкта.
        [TestMethod]
        public void Constructor_WithValidData_InitializesCorrectly()
        {
            var car = new ElectricCar(100, 2, 15, 60);

            Assert.AreEqual(100, car.Speed, "Швидкість має бути ініціалізована коректно.");
            Assert.AreEqual(2, car.TravelTime, "Час у дорозі має бути ініціалізований коректно.");
            Assert.AreEqual(15, car.FuelConsumption, "Витрата енергії має бути ініціалізована коректно.");
            Assert.AreEqual(60, car.BatteryCapacity, "Ємність батареї має бути збережена.");
        }

        // Тестування невалідної ємності батареї.
        [TestMethod]
        public void Constructor_WithZeroBatteryCapacity_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new ElectricCar(100, 2, 15, 0));
        }

        [TestMethod]
        public void BatteryCapacity_Negative_ThrowsArgumentException()
        {
            var car = new ElectricCar(100, 2, 15, 60);
            Assert.ThrowsException<ArgumentException>(() => car.BatteryCapacity = -10);
        }

        // Тестування невалідних даних базового класу.
        [TestMethod]
        public void Constructor_WithNegativeSpeed_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new ElectricCar(-100, 2, 15, 60));
        }

        // Відстань = швидкість * час.
        [TestMethod]
        public void CalculateTotalDistance_ReturnsSpeedTimesTime()
        {
            var car = new ElectricCar(80, 2.5, 15, 60);
            Assert.AreEqual(200, car.CalculateTotalDistance(), 0.0001);
        }

        // Електромобіль не створює викидів.
        [TestMethod]
        public void CalculateCO2Emission_ReturnsZero()
        {
            var car = new ElectricCar(100, 5, 15, 60);
            Assert.AreEqual(0, car.CalculateCO2Emission(), 0.0001);
        }

        // Запас ходу 60 / 15 * 100 = 400 км, маршрут 300 км - зарядки не потрібні.
        [TestMethod]
        public void CalculateChargesCount_ShortTrip_ReturnsZero()
        {
            var car = new ElectricCar(100, 3, 15, 60);
            Assert.AreEqual(0, car.CalculateChargesCount());
        }

        // Маршрут рівно на запас ходу (400 км) - зарядки не потрібні.
        [TestMethod]
        public void CalculateChargesCount_TripEqualToRange_ReturnsZero()
        {
            var car = new ElectricCar(100, 4, 15, 60);
            Assert.AreEqual(0, car.CalculateChargesCount());
        }

        // 800 км при запасі ходу 400 км - одна зарядка.
        [TestMethod]
        public void CalculateChargesCount_TwoRanges_ReturnsOne()
        {
            var car = new ElectricCar(100, 8, 15, 60);
            Assert.AreEqual(1, car.CalculateChargesCount());
        }

        // 1000 км при запасі ходу 400 км: ceil(2.5) - 1 = 2 зарядки.
        [TestMethod]
        public void CalculateChargesCount_LongTrip_ReturnsTwo()
        {
            var car = new ElectricCar(100, 10, 15, 60);
            Assert.AreEqual(2, car.CalculateChargesCount());
        }

        // Нульова витрата енергії - запас ходу необмежений.
        [TestMethod]
        public void CalculateChargesCount_ZeroConsumption_ReturnsZero()
        {
            var car = new ElectricCar(100, 10, 0, 60);
            Assert.AreEqual(0, car.CalculateChargesCount());
        }

        // Вартість поїздки (успадкований метод): 200 / 100 * 15 * 50 = 1500.
        [TestMethod]
        public void CalculateTripCost_ReturnsCorrectValue()
        {
            var car = new ElectricCar(100, 2, 15, 60);
            Assert.AreEqual(1500, car.CalculateTripCost(50), 0.0001);
        }

        [TestMethod]
        public void GetRoute_ContainsTypeKindAndDistance()
        {
            var car = new ElectricCar(100, 2, 15, 60);
            string route = car.GetRoute();

            StringAssert.Contains(route, "Електричний");
            StringAssert.Contains(route, "приміський");
            StringAssert.Contains(route, "200");
        }

        // Поліморфізм: об'єкт доступний і через базовий клас, і через інтерфейс.
        [TestMethod]
        public void ElectricCar_CanBeUsedThroughBaseClassAndInterface()
        {
            Vehicle vehicle = new ElectricCar(100, 2, 15, 60);
            IEmissionCalculable emission = new ElectricCar(100, 2, 15, 60);

            Assert.AreEqual(200, vehicle.CalculateTotalDistance(), 0.0001);
            Assert.AreEqual(0, emission.CalculateCO2Emission(), 0.0001);
        }
    }
}