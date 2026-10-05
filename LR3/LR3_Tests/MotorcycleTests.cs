using LR3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LR3_Tests
{
    [TestClass]
    public sealed class MotorcycleTests
    {
        [TestMethod]
        public void Constructor_WithValidData_InitializesCorrectly()
        {
            var moto = new Motorcycle(120, 1.5, 5.5, MotorcycleClass.Sport);

            Assert.AreEqual(120, moto.Speed);
            Assert.AreEqual(1.5, moto.TravelTime);
            Assert.AreEqual(5.5, moto.FuelConsumption);
            Assert.AreEqual(MotorcycleClass.Sport, moto.Class);
        }

        [TestMethod]
        public void Constructor_WithZeroSpeed_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Motorcycle(0, 1, 5, MotorcycleClass.Touring));
        }

        [TestMethod]
        public void Constructor_WithUnknownClass_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Motorcycle(100, 1, 5, (MotorcycleClass)42));
        }

        [TestMethod]
        public void CalculateTotalDistance_ReturnsSpeedTimesTime()
        {
            var moto = new Motorcycle(90, 2, 5, MotorcycleClass.Cruiser);
            Assert.AreEqual(180, moto.CalculateTotalDistance(), 0.0001);
        }

        // Вартість: 200 / 100 * 5 * 50 = 500 (без коефіцієнтів).
        [TestMethod]
        public void CalculateTripCost_ReturnsCorrectValue()
        {
            var moto = new Motorcycle(100, 2, 5, MotorcycleClass.Touring);
            Assert.AreEqual(500, moto.CalculateTripCost(50), 0.0001);
        }

        // Спортивний мотоцикл: 3.5 * 100 / 100 = 3.5 с.
        [TestMethod]
        public void CalculateAccelerationTime_Sport_ReturnsCorrectValue()
        {
            var moto = new Motorcycle(100, 1, 5, MotorcycleClass.Sport);
            Assert.AreEqual(3.5, moto.CalculateAccelerationTime(), 0.0001);
        }

        // Скутер розганяється повільніше за спортивний мотоцикл.
        [TestMethod]
        public void CalculateAccelerationTime_Scooter_IsSlowerThanSport()
        {
            var scooter = new Motorcycle(100, 1, 3, MotorcycleClass.Scooter);
            var sport = new Motorcycle(100, 1, 6, MotorcycleClass.Sport);

            Assert.IsTrue(scooter.CalculateAccelerationTime() > sport.CalculateAccelerationTime());
        }

        // Час розгону пропорційний швидкості: 6.5 * 50 / 100 = 3.25 с.
        [TestMethod]
        public void CalculateAccelerationTime_IsProportionalToSpeed()
        {
            var moto = new Motorcycle(50, 1, 4, MotorcycleClass.Cruiser);
            Assert.AreEqual(3.25, moto.CalculateAccelerationTime(), 0.0001);
        }

        [TestMethod]
        public void GetRoute_ContainsClassAndKind()
        {
            var moto = new Motorcycle(20, 1, 4, MotorcycleClass.Scooter);
            string route = moto.GetRoute();

            StringAssert.Contains(route, "Scooter");
            StringAssert.Contains(route, "міський");
        }
    }
}