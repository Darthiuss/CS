using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EngineType
{
    Petrol,   // бензиновий
    Diesel,   // дизельний
    Hybrid    // гібридний
}

namespace LR3
{
    public class Car : Vehicle
    {
        public EngineType EngineType { get; set; }

        public Car(double speed, double travelTime, double fuelConsumption, EngineType engineType)
            : base(speed, travelTime, fuelConsumption)
        {
            if (!Enum.IsDefined(typeof(EngineType), engineType))
            {
                throw new ArgumentException("Невідомий тип двигуна.", nameof(engineType));
            }
            EngineType = engineType;
        }

        public override double CalculateTotalDistance()
        {
            return Speed * TravelTime;
        }

        protected virtual double GetEngineCoefficient()
        {
            switch (EngineType)
            {
                case EngineType.Diesel: return 0.9;
                case EngineType.Hybrid: return 0.7;
                default: return 1.0;
            }
        }

        public override double CalculateTripCost(double fuelPrice)
        {
            return base.CalculateTripCost(fuelPrice) * GetEngineCoefficient();
        }

        public override string GetRoute()
        {
            return $"Автомобіль ({EngineType}): {GetRouteKind()} маршрут автошляхами, " +
                   $"відстань {CalculateTotalDistance():F1} км";
        }
    }
}