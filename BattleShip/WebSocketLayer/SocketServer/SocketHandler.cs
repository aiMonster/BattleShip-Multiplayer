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
        private async void appServer_NewMessageReceived(WebSocketSession session, string message)
        {           
            var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(message);
            if (baseMessage.Type == NotificationTypes.RandomGameRequest)
            {
                Console.WriteLine("New random user request");
                await RandomUserRequest(session);
            }
            else if(baseMessage.Type == NotificationTypes.CreateGame)
            {
                Console.WriteLine("Created new game with friend");
                await CreateGame(session);                
            }
            else if(baseMessage.Type == NotificationTypes.ParticipateGame)
            {
                Console.WriteLine("User wants to participate game");
               await ParticipateGame(session, JsonConvert.DeserializeObject<GameWithFriendRequest>(message));
            }
            else if(baseMessage.Type == NotificationTypes.ShipsPlaced)
            {
                Console.WriteLine("Someone ships are placed");
                await ShipsPlaced(session);
            }
            else if(baseMessage.Type == NotificationTypes.MoveMade)
            {
                Console.WriteLine("Someone made move");
                await MoveMade(session, JsonConvert.DeserializeObject<MoveCoordinates>(message));
            }
            else if(baseMessage.Type == NotificationTypes.MoveChecked)
            {
                Console.WriteLine("We checked move and sent responses for both users");
                await CheckMove(session, JsonConvert.DeserializeObject<ShotResult>(message));
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
            var game = _context.Games.FirstOrDefault(g => g.FirstUserId == session.SessionID || g.SecondUserId == session.SessionID);
            if(game != null)
            {
                if (game.FirstUserId == session.SessionID)
                {
                    SendMessage(game.SecondUserId, new BaseMessage(NotificationTypes.OpponentSurrended));
                }
                else
                {
                    SendMessage(game.FirstUserId, new BaseMessage(NotificationTypes.OpponentSurrended));
                }
                _context.Games.Remove(game);
            }
            
            Console.WriteLine();
            Console.WriteLine("Client disconnected! Sessions counter: " + appServer.SessionCount);
        }

        private void SendMessage(string id, BaseMessage message)
        {
            var session = appServer.GetAllSessions().FirstOrDefault(s => s.SessionID == id);
            if (session != null)
            {
                session.Send(JsonConvert.SerializeObject(message));
            }
        }
    }
}
