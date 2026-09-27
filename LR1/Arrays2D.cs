using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    internal class Arrays2D
    {
        public bool error = false;
        int x_length = 5;
        int y_length = 4;
        private int[,] a;
        //розмірність
        public int X_Length
        {
            get { return x_length; }
            set { x_length = value; }
        }
        public int Y_Length
        {
            get { return y_length; }
            set { y_length = value; }
        }
        //індексатор
        public int this[int i, int j]
        {
            get
            {
                if ((0 <= i) && (i < x_length) && (0 <= j) && (j < y_length))
                    return a[i,j];
                else
                {
                    error = true;
                    return 0;
                }
            }
            set
            {
                if ((0 <= i) && (i < x_length) && (0 <= j) && (j < y_length) && (value >= -10) && (value <= 10))
                    a[i,j] = value;
                else
                    error = true;
            }
        }
        //Конструктори
        public Arrays2D()
        {
            a = new int[x_length, y_length];
        }
        public Arrays2D(int[,] mas)
        {
            a = mas;
        }
        public Arrays2D(int x_size, int y_size)
        {
            x_length = x_size;
            y_length = y_size;
            a = new int[x_length, y_length];
            Random random = new Random();
            for (int i = 0; i < x_length; i++)
                for (int j = 0; j < y_length; j++)
                    this[i,j] = random.Next(-10, 10);
        }
        //Заміна на 111
        public void change()
        {
            for (int i = 0; i < x_length; i++)
                for (int j = 0; j < y_length; j++)
                    if(this[i, j] < 5)
                        this[i, j] = 111;
        }

        public int findMAX()
        {
            int max = -10000;
            for (int i = 0; i < x_length; i++)
                for (int j = 0; j < y_length; j++)
                    if(this[i, j] > max)
                        max = this[i, j];
            return max;
        }
    }
}
