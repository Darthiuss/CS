using LR3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LR3_Tests
{
    [TestClass]
    public sealed class TripAnalysisTests
    {
        // Порожній список - вартість 0.
        [TestMethod]
        public void GetTotalTripCost_EmptyList_ReturnsZero()
        {
            var analysis = new TripAnalysis();
            Assert.AreEqual(0, analysis.GetTotalTripCost(new List<ICostCalculable>(), 50));
        }

        // Автомобіль (800) + мотоцикл (500) = 1300.
        [TestMethod]
        public void GetTotalTripCost_CarAndMotorcycle_ReturnsSum()
        {
            var analysis = new TripAnalysis();
            var vehicles = new List<ICostCalculable>
            {
                new Car(100, 2, 8, EngineType.Petrol),
                new Motorcycle(100, 2, 5, MotorcycleClass.Touring)
            };

            Assert.AreEqual(1300, analysis.GetTotalTripCost(vehicles, 50), 0.0001);
        }

        // Поліморфізм: для кожного об'єкта викликається власна реалізація.
        [TestMethod]
        public void GetTotalTripCost_UsesPolymorphicCostOfEachVehicle()
        {
            var analysis = new TripAnalysis();
            var vehicles = new List<ICostCalculable>
            {
                new Car(100, 2, 8, EngineType.Hybrid),   // 560
                new Car(100, 2, 8, EngineType.Diesel)    // 720
            };

            Assert.AreEqual(1280, analysis.GetTotalTripCost(vehicles, 50), 0.0001);
        }

        [TestMethod]
        public void GetTotalTripCost_NullList_ThrowsArgumentNullException()
        {
            var analysis = new TripAnalysis();
            Assert.ThrowsException<ArgumentNullException>(() => analysis.GetTotalTripCost(null, 50));
        }

        [TestMethod]
        public void GetTotalTripCost_NegativeFuelPrice_ThrowsArgumentException()
        {
            var analysis = new TripAnalysis();
            Assert.ThrowsException<ArgumentException>(() => analysis.GetTotalTripCost(new List<ICostCalculable>(), -1));
        }

        // Поліморфізм через інтерфейс: 36.8 (авто) + 23 (мотоцикл) + 0 (електрокар) = 59.8.
        [TestMethod]
        public void GetTotalEmission_MixedVehicles_ReturnsSum()
        {
            var analysis = new TripAnalysis();
            var items = new List<IEmissionCalculable>
            {
                new Car(100, 2, 8, EngineType.Petrol),
                new Motorcycle(100, 2, 5, MotorcycleClass.Touring),
                new ElectricCar(100, 2, 15, 60)
            };

            Assert.AreEqual(60.06, analysis.GetTotalEmission(items), 0.0001);
        }

        [TestMethod]
        public void GetTotalEmission_EmptyList_ReturnsZero()
        {
            var analysis = new TripAnalysis();
            Assert.AreEqual(0, analysis.GetTotalEmission(new List<IEmissionCalculable>()), 0.0001);
        }

        [TestMethod]
        public void GetTotalEmission_NullList_ThrowsArgumentNullException()
        {
            var analysis = new TripAnalysis();
            Assert.ThrowsException<ArgumentNullException>(() => analysis.GetTotalEmission(null));
        }

        // Електро 200 / 100 * 15 * 50 = 1500, авто 800, разом 2300.
        [TestMethod]
        public void GetTotalTripCost_WithElectricCar_ReturnsSum()
        {
            var analysis = new TripAnalysis();
            var vehicles = new List<Vehicle>
            {
                new ElectricCar(100, 2, 15, 60),
                new Car(100, 2, 8, EngineType.Petrol)
            };

            Assert.AreEqual(2300, analysis.GetTotalTripCost(vehicles.ConvertAll(v => (ICostCalculable)v), 50), 0.0001);
        }
    }
}