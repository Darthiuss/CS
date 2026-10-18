using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3
{
    public interface IMovable
    {
        double Speed { get; set; }
        double TravelTime { get; set; }

        double CalculateTotalDistance();
        string GetRoute();
    }
}
