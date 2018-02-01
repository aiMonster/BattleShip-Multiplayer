using Common.DTO;
using Common.DTO.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Context
{
    public class MSContext
    {
        public List<string> RandomUsersList { get; set; }
        public List<GameWithFriendRequest> GamesWithFriends { get; set; }
        public List<GameInfo> Games { get; set; }

        public MSContext()
        {
            RandomUsersList = new List<string>();
            GamesWithFriends = new List<GameWithFriendRequest>();
            Games = new List<GameInfo>();
        }
    }
}
