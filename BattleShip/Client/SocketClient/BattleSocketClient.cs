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
        private WebSocket webSocketClient;
        private string url;
        private string protocol;
        private WebSocketVersion version;

        public void Start(string url, string protocol, WebSocketVersion version)
        {
            this.url = url;
            this.protocol = protocol;
            this.version = version;
            webSocketClient = new WebSocket(this.url, this.protocol, this.version);

            webSocketClient.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocketClient_Error);
            webSocketClient.Opened += new EventHandler(websocketClient_Opened);
            webSocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocketClient_MessageReceived);

            webSocketClient.Open();
            while (true) ;
        }
    }
}
