using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    internal class Arrays
    {
        public bool error = false;
        int[] a;
        int length = 5;
        //розмірність
        public int Length
        {
            get {  return length; }
            set { length = value; }
        }
        //індексатор
        public int this[int i]
        {
            get
            {
                if ((0 <= i) && (i < length))
                   return a[i];
                else
                {
                    error = true;
                    return 0;
                }
            }
            set
            {
                if ((0 <= i) && (i < length) && (value >= -100) && (value <= 100))
                    a[i] = value;
                else
                    error = true;
            }
        }
        //Конструктори
        public Arrays()
        {
            a = new int[length];
        }
        public Arrays(int[] mas)
        {
            a = mas;
        }
        public Arrays(int size)
        {
            Length = size;
            a = new int[length];
            Random random = new Random();
            for (int i = 0; i < length; i++)
                this[i] = random.Next(-50, 50);
        }
        //Середнє
        public double mean()
        {
            double sum = 0;
            foreach (int i in a)
                sum += i;
            return sum / length;
        }
    }
}
