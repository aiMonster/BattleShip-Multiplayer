using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Common.DTO.Requests
{
    public class GameWithFriendRequest : BaseMessage
    {
        public string CreatorId { get; set; }        
        public string Password { get; set; }
        public int GameId { get; set; }

        public static int GlobalGameId;

        public GameWithFriendRequest() { }

        public GameWithFriendRequest(NotificationTypes type) :base(type)
        {
            GameId = Interlocked.Increment(ref GlobalGameId);
        }

    }
}
