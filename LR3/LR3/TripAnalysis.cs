using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3
{
    public class TripAnalysis 
    {
        public double GetTotalTripCost(List<ICostCalculable> vehicles, double fuelPrice)
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

        public double GetTotalEmission(List<IEmissionCalculable> vehicles)
        {
            if (vehicles == null)
            {
                throw new ArgumentNullException(nameof(vehicles));
            }
            double totalEmission = 0;
            foreach (IEmissionCalculable vehicle in vehicles)
            {
                totalEmission += vehicle.CalculateCO2Emission();
            }
            return totalEmission;
        }

        public double GetMinimalPriceTrip(List<Vehicle> vehicles, double fuelPrice)
        {
            double minPrice = 999999;
            foreach (Vehicle vehicle in vehicles)
            {
                double current_price = vehicle.CalculateTripCost(fuelPrice);
                if (minPrice > current_price)
                    minPrice = current_price;
            }
            return minPrice;
        }
    }
}
