using Common.DTO;
using Common.DTO.Requests;
using Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;


namespace ConsoleClient.SocketClient
{
    public partial class BattleSocketClient
    {       
        private async void websocketClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {            
            var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(e.Message);
            if (baseMessage.Type == NotificationTypes.WatingForShips)
            {                
                Console.WriteLine("Waiting for ships");
                await PutShips();
            }
            else if(baseMessage.Type == NotificationTypes.GameCreated)
            {
                Console.WriteLine("GameCreated");
                await GameCreated(JsonConvert.DeserializeObject<GameWithFriendRequest>(e.Message));
            }
            else if(baseMessage.Type == NotificationTypes.GameNotFound || baseMessage.Type == NotificationTypes.GamePasswordNotValid)
            {
                Console.WriteLine("Game not found or password not valid");
                await StartGame();
            }
            else if(baseMessage.Type == NotificationTypes.OpponentSurrended)
            {                
                Console.WriteLine("Opponent surrended");                
                await StartGame();
            }
            else if(baseMessage.Type == NotificationTypes.WaitingForOpponentShips)
            {
                Console.WriteLine("Waiting when opponent will place his ships");
            }
            else if(baseMessage.Type == NotificationTypes.Won)
            {
                Console.WriteLine("Congratulations, you won");
            }
            else if(baseMessage.Type == NotificationTypes.Lost)
            {
                Console.WriteLine("Loser, you just lost!");
            }
            else if(baseMessage.Type == NotificationTypes.YourMove)
            {
                Console.WriteLine("Making move");
                await MakeMove();
            }
            else if(baseMessage.Type == NotificationTypes.OpponentMove)
            {
                Console.WriteLine("Waiting while opponent will make his move");
            }
            else if(baseMessage.Type == NotificationTypes.MoveMade)
            {
                Console.WriteLine("we are checking opponent move");
               await CheckMove(JsonConvert.DeserializeObject<MoveCoordinates>(e.Message));
            }
            else if(baseMessage.Type == NotificationTypes.MoveChecked)
            {
                Console.WriteLine("Our move was checked");
               await MoveChecked(JsonConvert.DeserializeObject<ShotResult>(e.Message));
            }
        }



        public void SendMessage(BaseMessage message)
        {
            webSocketClient.Send(JsonConvert.SerializeObject(message));                     
        }
        
        private async void websocketClient_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Client successfully connected.");
            await StartGame();
        }

        private void websocketClient_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.GetType() + ": " + e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);

            if (e.Exception.InnerException != null)
            {
                Console.WriteLine(e.Exception.InnerException.GetType());
            }

            return;
        }
    }
}
