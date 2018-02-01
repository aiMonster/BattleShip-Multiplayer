using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    public class GameInfo
    {
        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }
        public int UsersReadyToGame { get; set; }
    }
}
