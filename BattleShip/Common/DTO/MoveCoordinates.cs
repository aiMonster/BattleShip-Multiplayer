using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    public class MoveCoordinates : BaseMessage
    {
        public int A { get; set; }
        public int B { get; set; }

        public MoveCoordinates() { }

        public MoveCoordinates(NotificationTypes type) : base(type)
        {

        }

        public MoveCoordinates(NotificationTypes type, MoveCoordinates coordinates) : base(type)
        {
            A = coordinates.A;
            B = coordinates.B;
        }
    }
}
