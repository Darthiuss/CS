using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    public class Triangle
    {
        private double a;
        private double b;
        private double c;

        public double A { get => a; set => a = value; }
        public double B { get => b; set => b = value; }
        public double C { get => c; set => c = value; }

        public Triangle()
        {
            this.a = 3;
            this.b = 4;
            this.c = 5;
        }

        public Triangle(double side)
        {
            if (side <= 0)
                throw new ArgumentException("Сторона повинна бути більшою за 0.");
            this.a = side;
            this.b = side;
            this.c = side;
        }

        public Triangle(double a, double b, double c)
        {
            if (a <= 0 || b <= 0 || c <= 0)
                throw new ArgumentException("Сторони повинні бути додатними числами.");

            if (a + b <= c || a + c <= b || b + c <= a)
                throw new ArgumentException("Трикутник з такими сторонами не існує.");

            this.a = a;
            this.b = b;
            this.c = c;
        }

        public double CalculateArea()
        {
            double p = (a + b + c) / 2.0;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }

        public string GetTriangleType()
        {
            double[] sides = { a, b, c };
            Array.Sort(sides);
            double sideA = sides[0];
            double sideB = sides[1];
            double maxC = sides[2];

            double cSq = maxC * maxC;
            double abSq = sideA * sideA + sideB * sideB;
            double eps = 1e-7;

            if (Math.Abs(cSq - abSq) < eps)
            {
                return "Прямокутний";
            }
            else if (cSq < abSq)
            {
                return "Гострокутний";
            }
            else
            {
                return "Тупокутний";
            }
        }
    }
}