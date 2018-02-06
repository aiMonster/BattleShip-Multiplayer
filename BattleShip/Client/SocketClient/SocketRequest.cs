using Client;
using Common.DTO;
using Common.DTO.Requests;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleClient.SocketClient
{
    public partial class BattleSocketClient
    {
        //done but need customization
        private async Task StartGame()
        {
            Console.WriteLine(new string('=', 25));
            Console.WriteLine("Choose mode of game:\n1 - Play with random player\n2 - Play with your friend");
            int answer = 0;
            do
            {
                Console.WriteLine("\nYour answer: ");
                try
                {
                    answer = Convert.ToInt32(Console.ReadLine());
                }
                catch { Console.WriteLine("Idiot -_-"); }
            }
            while (answer < 1 || answer > 2);

            if (answer == 1)
            {
                Console.WriteLine("\n\tYou chosed game with random player, just wait for him");
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
                    Console.WriteLine("Enter id - ");
                    int id = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter password - ");
                    string pas = Console.ReadLine();
                    SendMessage(new GameWithFriendRequest(NotificationTypes.ParticipateGame) { GameId = id, Password = pas });
                }
            }           
        }

        private async Task PutShips()
        {
            Console.WriteLine(new string('=', 25));
            Console.WriteLine("We connected to the opponent, place your ships");            
            await Field.FillField();            
            SendMessage(new BaseMessage(NotificationTypes.ShipsPlaced));
        }

        //done but need customization
        private async Task MakeMove()
        {
            Console.Clear();
            Console.WriteLine(new string('=', 25));
            Field.ShowFields();
            MoveCoordinates coordinates = new MoveCoordinates(NotificationTypes.MoveMade);
            start:
            try
            {
                Console.WriteLine("Enter a(0-9): ");
                coordinates.A = Convert.ToInt32(Console.ReadLine()) + 4;

                Console.WriteLine("Enter b(q-p): ");
                var ch = Convert.ToChar(Console.ReadLine());
                switch (ch)
                {
                    case 'q':
                        coordinates.B = 4;
                        break;
                    case 'w':
                        coordinates.B = 5;
                        break;
                    case 'e':
                        coordinates.B = 6;
                        break;
                    case 'r':
                        coordinates.B = 7;
                        break;
                    case 't':
                        coordinates.B = 8;
                        break;
                    case 'y':
                        coordinates.B = 9;
                        break;
                    case 'u':
                        coordinates.B = 10;
                        break;
                    case 'i':
                        coordinates.B = 11;
                        break;
                    case 'o':
                        coordinates.B = 12;
                        break;
                    case 'p':
                        coordinates.B = 13;
                        break;
                }

            }
            catch
            {
                goto start;
            }


            if (Field.DidWeShotHere(coordinates))
            {
                Console.WriteLine("You already shoot here, try again");
                goto start;
            }

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
            Console.WriteLine(new string('=', 25));
            Console.WriteLine("Info of game you have created:\nId - " + game.GameId + "\nPassword - " + game.Password);
        }

        //seems to be good
        private async Task MoveChecked(ShotResult result)
        {
            Field.MarkOurShot(result);
        }
    }
}
