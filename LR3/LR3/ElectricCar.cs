using System;

namespace LR3
{
    public class ElectricCar : Vehicle, IEmissionCalculable
    {
        private double batteryCapacity;

        public double BatteryCapacity
        {
            get { return batteryCapacity; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Ємність батареї повинна бути більшою за нуль.", nameof(value));
                }
                batteryCapacity = value;
            }
        }

        public ElectricCar(double speed, double travelTime, double fuelConsumption, double batteryCapacity)
            : base(speed, travelTime, fuelConsumption)
        {
            BatteryCapacity = batteryCapacity;
        }

        public override double CalculateTotalDistance()
        {
            return Speed * TravelTime;
        }

        public double CalculateCO2Emission()
        {
            return 0;
        }

        private double GetRange()
        {
            if (FuelConsumption == 0) return double.PositiveInfinity;
            return BatteryCapacity / FuelConsumption * 100.0;
        }

        public int CalculateChargesCount()
        {
            double distance = CalculateTotalDistance();
            double range = GetRange();

            if (distance <= range) return 0;
            return (int)Math.Ceiling(distance / range) - 1;
        }

        public override string GetRoute()
        {
            return $"Електричний автомобіль: {GetRouteKind()} маршрут автошляхами, " +
                   $"відстань {CalculateTotalDistance():F1} км";
        }
    }
}