using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO.Requests
{
    public class ShotResult : MoveCoordinates
    {       
        public ShotResultTypes shotResult { get; set; }

        public ShotResult() { }
        public ShotResult(NotificationTypes type): base(type) { }
        public ShotResult(NotificationTypes type, MoveCoordinates coordinates) :base(type)
        {
            A = coordinates.A;
            B = coordinates.B;
        }

        public ShotResult(NotificationTypes type, ShotResult result) : base(type)
        {
            A = result.A;
            B = result.B;
            shotResult = result.shotResult;
        }
    }
}
