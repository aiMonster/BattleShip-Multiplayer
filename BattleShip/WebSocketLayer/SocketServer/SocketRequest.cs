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
        private async Task RandomUserRequest(WebSocketSession session)
        {            
            if (_context.RandomUsersList.Count == 0)
            {
                _context.RandomUsersList.Add(session.SessionID);
            }
            else
            {
                var opponent = _context.RandomUsersList[0];
                _context.RandomUsersList.Remove(opponent);
                _context.Games.Add(new GameInfo() { FirstUserId = session.SessionID, SecondUserId = opponent });

                SendMessage(session.SessionID, new BaseMessage(NotificationTypes.WatingForShips));
                SendMessage(opponent, new BaseMessage(NotificationTypes.WatingForShips));
            }
        }       

        private async Task CreateGame(WebSocketSession session)
        {            
            var game = new GameWithFriendRequest(NotificationTypes.GameCreated);
            game.CreatorId = session.SessionID;
            Random rnd = new Random();
            game.Password = rnd.Next(1000, 9999).ToString();

            _context.GamesWithFriends.Add(game);
            SendMessage(session.SessionID, game);
        }

        private async Task ParticipateGame(WebSocketSession session, GameWithFriendRequest request)
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

            _context.Games.Add(new GameInfo() { FirstUserId = game.CreatorId, SecondUserId = session.SessionID });
            _context.GamesWithFriends.Remove(game);

            SendMessage(session.SessionID, new BaseMessage(NotificationTypes.WatingForShips));
            SendMessage(game.CreatorId, new BaseMessage(NotificationTypes.WatingForShips));            
        }

        private async Task ShipsPlaced(WebSocketSession session)
        {
            var game = _context.Games.FirstOrDefault(g => g.FirstUserId == session.SessionID || g.SecondUserId == session.SessionID);
            if(game == null)
            {
                SendMessage(session.SessionID, new BaseMessage(NotificationTypes.OpponentSurrended));
                return; 
            }

            game.UsersReadyToGame++;
            if(game.UsersReadyToGame < 2)
            {
                SendMessage(session.SessionID, new BaseMessage(NotificationTypes.WaitingForOpponentShips));
                return;
            }

            SendMessage(game.FirstUserId, new BaseMessage(NotificationTypes.YourMove));
            SendMessage(game.SecondUserId, new BaseMessage(NotificationTypes.OpponentMove));
        }
                
        private async Task MoveMade(WebSocketSession session, MoveCoordinates coordintes)
        {
            var game = _context.Games.FirstOrDefault(g => g.FirstUserId == session.SessionID || g.SecondUserId == session.SessionID);
            if(game == null)
            {
                SendMessage(session.SessionID, new BaseMessage(NotificationTypes.OpponentSurrended));
                return;
            }

            if(game.FirstUserId == session.SessionID)
            {
                SendMessage(game.SecondUserId, new MoveCoordinates(NotificationTypes.MoveMade, coordintes));
            }
            else
            {
                SendMessage(game.FirstUserId, new MoveCoordinates(NotificationTypes.MoveMade, coordintes));
            }

        }

        private async Task CheckMove(WebSocketSession session, ShotResult shot)
        {
            var game = _context.Games.FirstOrDefault(g => g.FirstUserId == session.SessionID || g.SecondUserId == session.SessionID);
            if(game == null)
            {
                SendMessage(session.SessionID, new BaseMessage(NotificationTypes.OpponentSurrended));
                return;
            }
            var opponentId = session.SessionID == game.FirstUserId ? game.SecondUserId : game.FirstUserId;

            if(shot.shotResult == ShotResultTypes.MovePast)
            {
                SendMessage(session.SessionID, new BaseMessage(NotificationTypes.YourMove));

                SendMessage(opponentId, new ShotResult(NotificationTypes.MoveChecked, shot));
                SendMessage(opponentId, new BaseMessage(NotificationTypes.OpponentMove));
            }
            else if(shot.shotResult == ShotResultTypes.MoveInjured)
            {
                SendMessage(session.SessionID, new BaseMessage(NotificationTypes.OpponentMove));

                SendMessage(opponentId, new ShotResult(NotificationTypes.MoveChecked, shot));
                SendMessage(opponentId, new BaseMessage(NotificationTypes.YourMove));
            }
            else if(shot.shotResult == ShotResultTypes.MoveKilled)
            {
                SendMessage(session.SessionID, new BaseMessage(NotificationTypes.OpponentMove));

                SendMessage(opponentId, new ShotResult(NotificationTypes.MoveChecked, shot));
                SendMessage(opponentId, new BaseMessage(NotificationTypes.YourMove));
            }
            else if(shot.shotResult == ShotResultTypes.MoveFatal)
            {
                SendMessage(session.SessionID, new BaseMessage(NotificationTypes.Lost));

                SendMessage(opponentId, new ShotResult(NotificationTypes.MoveChecked, shot));
                SendMessage(opponentId, new BaseMessage(NotificationTypes.Won));
            }
        }

    }

}
