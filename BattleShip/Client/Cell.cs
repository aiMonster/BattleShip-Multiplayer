using Client.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Cell
    {
        public int A { get; set; }
        public int B { get; set; }
        public CellStateTypes cellState { get; set; }

        public Cell(int a, int b)
        {
            A = a;
            B = b;
            cellState = CellStateTypes.Alive;
        }
    }
}
