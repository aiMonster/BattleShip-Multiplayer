using ConsoleClient.SocketClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;

namespace ConsoleClient
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("hello");
            BattleSocketClient client = new BattleSocketClient();
            client.Start("ws://localhost:8080", "basic", WebSocketVersion.Rfc6455);

            Console.ReadLine();

        }
    }
}
