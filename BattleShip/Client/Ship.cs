using Client.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Ship
    {
        public int Length { get; set; }
        public ShipStateTypes shipState { get; set; }
        public Cell[] cells;

        public Ship(int l)
        {
            Length = l;
            shipState = ShipStateTypes.Alive;
            cells = new Cell[Length];
        }

        public void SetCoordinate(int number, int a, int b)
        {
            cells[number] = new Cell(a, b);            
        }
    }
}
