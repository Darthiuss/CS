using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3
{
    public abstract class Vehicle
    {
        private double speed;
        private double travelTime;
        private double fuelConsumption;

        public double Speed
        {
            get { return speed; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Швидкість повинна бути більшою за нуль.", nameof(value));
                }
                speed = value;
            }
        }

        public double TravelTime
        {
            get { return travelTime; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Час у дорозі повинен бути більшим за нуль.", nameof(value));
                }
                travelTime = value;
            }
        }

        public double FuelConsumption
        {
            get { return fuelConsumption; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Витрата пального не може бути від'ємною.", nameof(value));
                }
                fuelConsumption = value;
            }
        }

        public Vehicle(double speed, double travelTime, double fuelConsumption)
        {
            Speed = speed;
            TravelTime = travelTime;
            FuelConsumption = fuelConsumption;
        }

        public abstract double CalculateTotalDistance();

        public abstract string GetRoute();

        public virtual double CalculateTripCost(double fuelPrice)
        {
            if (fuelPrice < 0)
            {
                throw new ArgumentException("Ціна пального не може бути від'ємною.", nameof(fuelPrice));
            }
            return CalculateTotalDistance() / 100.0 * FuelConsumption * fuelPrice;
        }

        protected string GetRouteKind()
        {
            double distance = CalculateTotalDistance();
            if (distance <= 30) return "міський";
            if (distance <= 300) return "приміський";
            return "міжміський";
        }
    }
}