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
            Console.WriteLine("\n\t\tWelcome to ButtleShip Multiplayer!\n");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Cyan;            
            BattleSocketClient client = new BattleSocketClient();
            Console.WriteLine("Enter Ip Adress: ");
            string ip = Console.ReadLine();
            client.Start("ws://" + ip + ":8080", "basic", WebSocketVersion.Rfc6455);

            Console.ReadLine();

        }
    }
}
