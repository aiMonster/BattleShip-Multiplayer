using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Extensions
{
    public static class ArrayExtension
    {
        public static void Fill<T>(this T[,] originalArray, T shipC, T waterC)
        {
            for(int a = 0; a < 18; a++)
            {
                for(int b = 0; b < 18; b++)
                {
                    originalArray[a, b] = shipC;
                }
            }

            for (int a = 3; a < 15; a++)
            {
                for (int b = 3; b < 15; b++)
                {
                    originalArray[a, b] = waterC;
                }
            }
        }
    }
}
