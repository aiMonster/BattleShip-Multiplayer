using DataAcessLayer.Context;
using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketLayer.General.Interfaces;

namespace WebSocketLayer.SocketServer
{
    public partial class BattleSocketServer : IBattleSocketServer
    {
        private WebSocketServer appServer;
        private MSContext _context;
        
        public BattleSocketServer(MSContext context)
        {
            _context = context;
           
            Console.WriteLine("Setuping WebSocketServer!");
            appServer = new WebSocketServer();
            if (!appServer.Setup(8080)) //Setup with listening port
            {
                return;
            }
            appServer.NewSessionConnected += new SessionHandler<WebSocketSession>(appServer_NewSessionConnected);
            appServer.SessionClosed += new SessionHandler<WebSocketSession, CloseReason>(appServer_SessionClosed);
            appServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(appServer_NewMessageReceived);
            
            if (!appServer.Start())
            {
                return;
            }
            
        }        
    }
}
