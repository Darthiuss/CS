using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    public class Task2
    {
        private int a;
        private int b;

        public int A { get => a; set => a = value; }
        public int B { get => b; set => b = value; }

        public Task2()
        {
            this.a = 0;
            this.b = 0;
        }

        public Task2(int b)
        {
            this.a = 0;
            this.b = b;
        }

        public Task2(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public long CalculateSum()
        {
            if (a > b)
            {
                throw new ArgumentOutOfRangeException("Параметр 'a' повинен бути меншим або дорівнювати 'b'.");
            }

            long sum = 0;
            for (int i = a; i <= b; i++)
            {
                if (i % 11 == 0 && ((i % 8 + 8) % 8 == 5))
                {
                    sum += i;
                }
            }

            return sum;
        }
    }
}