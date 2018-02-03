using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Extensions
{
    public static class ArrayExtension
    {
        public static void Fill<T>(this T[,] originalArray, T value)
        {
            for(int a = 0; a < 10; a++)
            {
                for(int b = 0; b < 10; b++)
                {
                    originalArray[a, b] = value;
                }
            }
        }
    }
}
