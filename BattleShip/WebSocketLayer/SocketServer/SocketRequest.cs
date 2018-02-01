using Common.DTO;
using Common.DTO.Requests;
using Common.Enums;
using Newtonsoft.Json;
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
        private void RandomUserRequest(WebSocketSession session)
        {
            Console.WriteLine("new random user request");
            if (_context.RandomUsersList.Count == 0)
            {
                _context.RandomUsersList.Add(session.SessionID);
            }
            else
            {
                var opponent = _context.RandomUsersList[0];
                _context.RandomUsersList.Remove(opponent);              

                SendMessage(session.SessionID, new BaseMessage(NotificationTypes.WatingForShips));
                SendMessage(opponent, new BaseMessage(NotificationTypes.WatingForShips));
            }
        }

        private void CreateGame(WebSocketSession session)
        {
            var game = new GameWithFriendRequest(NotificationTypes.GameCreated);
            game.CreatorId = session.SessionID;
            game.Password = "1234";

            _context.GamesWithFriends.Add(game);
            SendMessage(session.SessionID, game);
        }

        private void ParticipateGame(WebSocketSession session, GameWithFriendRequest request)
        {
            var game = _context.GamesWithFriends.FirstOrDefault(g => g.GameId == request.GameId);
            if(game == null)
            {
                //game not found
                SendMessage(session.SessionID, new BaseMessage(NotificationTypes.GameNotFound));
                return;
            }
            else if (game.Password != request.Password)
            {
                //password is not valid
                SendMessage(session.SessionID, new BaseMessage(NotificationTypes.GamePasswordNotValid));
                return;
            }

            _context.Games.Add(new GameInfo() { FirstUserId = request.CreatorId, SecondUserId = session.SessionID });
            _context.GamesWithFriends.Remove(game);

            SendMessage(session.SessionID, new BaseMessage(NotificationTypes.WatingForShips));
            SendMessage(game.CreatorId, new BaseMessage(NotificationTypes.WatingForShips));            
        }

        private void SendMessage(string id, BaseMessage message)
        {
            var session = appServer.GetAllSessions().FirstOrDefault(s => s.SessionID == id);
            if(session != null)
            {
                session.Send(JsonConvert.SerializeObject(message));
            }
         }
    }
    
}
