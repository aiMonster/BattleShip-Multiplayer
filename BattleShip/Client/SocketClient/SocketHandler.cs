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
        private void websocketClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {            
            var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(e.Message);
            if (baseMessage.Type == NotificationTypes.WatingForShips)
            {                
                Console.WriteLine("Waiting for ships");                
            }
            else if(baseMessage.Type == NotificationTypes.GameCreated)
            {
                Console.WriteLine("GameCreated");
                GameCreated(JsonConvert.DeserializeObject<GameWithFriendRequest>(e.Message));
            }
            else if(baseMessage.Type == NotificationTypes.GameNotFound || baseMessage.Type == NotificationTypes.GamePasswordNotValid)
            {
                Console.WriteLine("Game not found or password not valid");
                StartGame();
            }
        }



        public void SendMessage(BaseMessage message)
        {
            webSocketClient.Send(JsonConvert.SerializeObject(message));                     
        }
        
        private void websocketClient_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Client successfully connected.");
            StartGame();
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
