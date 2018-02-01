using Common.DTO;
using Common.DTO.Requests;
using Common.Enums;
using Newtonsoft.Json;
using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketLayer.SocketServer
{
    public partial class BattleSocketServer
    {
        private void appServer_NewMessageReceived(WebSocketSession session, string message)
        {           
            var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(message);
            if (baseMessage.Type == NotificationTypes.RandomGameRequest)
            {
                RandomUserRequest(session);
            }
            else if(baseMessage.Type == NotificationTypes.CreateGame)
            {
                CreateGame(session);                
            }
            else if(baseMessage.Type == NotificationTypes.ParticipateGame)
            {
                ParticipateGame(session, JsonConvert.DeserializeObject<GameWithFriendRequest>(message));
            }
        }

        private void appServer_NewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine("New session connected! Sessions counter: " + appServer.SessionCount);
        }

        private void appServer_SessionClosed(WebSocketSession session, CloseReason value)
        {           
            _context.RandomUsersList.Remove(session.SessionID);
            _context.GamesWithFriends.Remove(_context.GamesWithFriends.FirstOrDefault(g => g.CreatorId == session.SessionID));

            Console.WriteLine();
            Console.WriteLine("Client disconnected! Sessions counter: " + appServer.SessionCount);
        }
    }
}
