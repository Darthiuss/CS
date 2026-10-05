using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum MotorcycleClass
{
    Scooter,  // скутер
    Cruiser,  // круїзер
    Touring,  // туристичний
    Sport     // спортивний
}

namespace LR3
{
    public class Motorcycle : Vehicle
    {
        public MotorcycleClass Class { get; set; }

        public Motorcycle(double speed, double travelTime, double fuelConsumption, MotorcycleClass motorcycleClass)
            : base(speed, travelTime, fuelConsumption)
        {
            if (!Enum.IsDefined(typeof(MotorcycleClass), motorcycleClass))
            {
                throw new ArgumentException("Невідомий клас мотоцикла.", nameof(motorcycleClass));
            }
            Class = motorcycleClass;
        }

        public override double CalculateTotalDistance()
        {
            return Speed * TravelTime;
        }

        protected virtual double GetBaseAccelerationTime()
        {
            switch (Class)
            {
                case MotorcycleClass.Sport: return 3.5;
                case MotorcycleClass.Touring: return 5.0;
                case MotorcycleClass.Cruiser: return 6.5;
                default: return 12.0;
            }
        }

        public virtual double CalculateAccelerationTime()
        {
            return GetBaseAccelerationTime() * Speed / 100.0;
        }

        public override string GetRoute()
        {
            return $"Мотоцикл ({Class}): {GetRouteKind()} маршрут з об'їздом заторів, " +
                   $"відстань {CalculateTotalDistance():F1} км";
        }
    }
}