using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    public class MoveCoordinates : BaseMessage
    {
        public int X { get; set; }
        public int Y { get; set; }

        public MoveCoordinates() { }

        public MoveCoordinates(NotificationTypes type) : base(type)
        {

        }

        public MoveCoordinates(NotificationTypes type, MoveCoordinates coordinates) : base(type)
        {
            X = coordinates.X;
            Y = coordinates.Y;
        }
    }
}
