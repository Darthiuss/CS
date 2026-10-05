using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3
{
    public class TripAnalysis
    {
        public double GetTotalTripCost(List<Vehicle> vehicles, double fuelPrice)
        {
            if (vehicles == null)
            {
                throw new ArgumentNullException(nameof(vehicles));
            }
            if (fuelPrice < 0)
            {
                throw new ArgumentException("Ціна пального не може бути від'ємною.", nameof(fuelPrice));
            }

            double total = 0;
            foreach (Vehicle vehicle in vehicles)
            {
                total += vehicle.CalculateTripCost(fuelPrice);
            }
            return total;
        }
    }
}
