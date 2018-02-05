using Client;
using Common.DTO;
using Common.DTO.Requests;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient.SocketClient
{
    public partial class BattleSocketClient
    {
        //done but need customization
        private async Task StartGame()
        {
            Console.WriteLine("Enter 1 to play with random player and 2 to play with your friend and 3 to go away");
            int answer = 0;
            do
            {
                Console.WriteLine("Your answer: ");
                try
                {
                    answer = Convert.ToInt32(Console.ReadLine());
                }
                catch { Console.WriteLine("Idiot -_-"); }
            }
            while (answer < 1 || answer > 3);

            if (answer == 1)
            {
                Console.WriteLine("You chosed game with random player");
                SendMessage(new BaseMessage(NotificationTypes.RandomGameRequest));
            }
            else if (answer == 2)
            {
                Console.WriteLine("You chosed game with your friend");
                Console.WriteLine("Do you want to create(1) or participate(2) game: ");
                int an = Convert.ToInt32(Console.ReadLine());
                if (an == 1)
                {
                   SendMessage(new BaseMessage(NotificationTypes.CreateGame));
                }
                else if (an == 2)
                {
                    Console.WriteLine("enter id - ");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("enter password - ");
                    string pas = Console.ReadLine();
                    SendMessage(new GameWithFriendRequest(NotificationTypes.ParticipateGame) { GameId = id, Password = pas });
                }
            }
            else
            {
                Console.WriteLine("You are going away! Just close app!");
            }
        }

        private async Task PutShips()
        {
            await Field.FillField();            
            SendMessage(new BaseMessage(NotificationTypes.ShipsPlaced));
        }

        //done but need customization
        private async Task MakeMove()
        {
            Field.ShowFields();
            Console.WriteLine("enter a: ");
            MoveCoordinates coordinates = new MoveCoordinates(NotificationTypes.MoveMade);
            coordinates.A = Convert.ToInt32(Console.ReadLine()) + 4;

            Console.WriteLine("enter b: ");
            coordinates.B = Convert.ToInt32(Console.ReadLine()) + 4;
            SendMessage(coordinates);            
        }

        //looks to be done
        private async Task CheckMove(MoveCoordinates coordinates)
        {
            var result = await Field.CheckShot(coordinates);
            SendMessage(new ShotResult(NotificationTypes.MoveChecked, coordinates) { shotResult = result });
        }

        //done but need customization
        private async Task GameCreated(GameWithFriendRequest game)
        {
            Console.WriteLine("Id - " + game.GameId + ", password - " + game.Password);
        }

        //seems to be good
        private async Task MoveChecked(ShotResult result)
        {
            Field.MarkOurShot(result);
        }
    }
}
