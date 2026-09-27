using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    public class Task1
    {
        private int a;
        private int b;
        private int c;

        public int A { get => a; set => a = value; }
        public int B { get => b; set => b = value; }
        public int C { get => c; set => c = value; }

        public Task1()
        {
            this.a = 0;
            this.b = 0;
            this.c = 0;
        }

        public Task1(int a)
        {
            this.a = a;
            this.b = 0;
            this.c = 0;
        }

        public Task1(int a, int b, int c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public long CalculateSumOfCubes()
        {
            long sum = 0;

            if (a % 7 == 0) sum += (long)Math.Pow(a, 3);
            if (b % 7 == 0) sum += (long)Math.Pow(b, 3);
            if (c % 7 == 0) sum += (long)Math.Pow(c, 3);

            return sum;
        }
    }
}