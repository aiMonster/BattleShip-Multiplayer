using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Enums;
using Client.Extensions;
using Common.DTO;
using Common.DTO.Requests;
using Common.Enums;

namespace Client
{
    public static class Game
    {
        private const char freeCell = '~';
        private const char shipClell = 'O';
        private const char shotedCell = ' ';
        private const char injuredCell = 'X';


        private static char[,] myField = new char[10,10];
        private static char[,] opponentField = new char[10, 10];

        static Game()
        {           
            myField.Fill(freeCell);
            opponentField.Fill(freeCell);            
        }

        //will work but we don't check is it killed
        public static async Task<ShotResultTypes> CheckShot(MoveCoordinates coordinates)
        {
            if(myField[coordinates.Y, coordinates.X] == freeCell)
            {
                myField[coordinates.Y, coordinates.X] = shotedCell;
                return ShotResultTypes.MovePast;
            }            
            else if(myField[coordinates.Y, coordinates.X] == shipClell)
            {
                myField[coordinates.Y, coordinates.X] = injuredCell;
                return ShotResultTypes.MoveInjured;
            }
            myField[coordinates.Y, coordinates.X] = injuredCell;
            return ShotResultTypes.MovePast;
        }

        //works only by yourself and only with one ship
        public static async Task FillField()
        {
            Console.WriteLine("Do you want to fill field by yourself(1) or by random(2)? -");
            int answer = Convert.ToInt32(Console.ReadLine());
            switch(answer)
            {
                case 1:
                    await FillByPlayer();
                    break;
                case 2:
                    FillByRandom();
                    break;
            }
        }

        private async static Task FillByPlayer()
        {
            Console.WriteLine("Filling by you:");
            await PutShip(4);
            await ShowFields();
            //PutShip(3);
            //ShowFields();
            //PutShip(3);
            //PutShip(2);
            //PutShip(2);
            //PutShip(2);
            //PutShip(1);
            //PutShip(1);
            //PutShip(1);
            //PutShip(1);
        }

        private static void FillByRandom()
        {
            Console.WriteLine("Filling by random:");
        }

        //done but we don't say when it is killed
        public static void MarkOurShot(ShotResult shot)
        {
            switch (shot.shotResult)
            {
                case ShotResultTypes.MoveInjured:
                case ShotResultTypes.MoveKilled:
                case ShotResultTypes.MoveFatal:
                    opponentField[shot.Y, shot.X] = injuredCell;
                    break;
                case ShotResultTypes.MovePast:
                    opponentField[shot.Y, shot.X] = freeCell;
                    break;
            }

        }

        private static async Task PutShip(int length)
        {
            bool isOkay = false;
            while(!isOkay)
            {
                Console.WriteLine("Ener X coordinate:");
                int xCoordinate = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Ener Y coordinate:");
                int yCoordinate = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter direction:");
                DirectionTypes direction = (DirectionTypes)Convert.ToInt32(Console.ReadLine());

                if(direction == DirectionTypes.Up)
                {


                    for(int i = 0; i < length; i++)
                    {
                        myField[yCoordinate - i, xCoordinate] = shipClell;
                    }
                }
                else if(direction == DirectionTypes.Right)
                {


                    for (int i = 0; i < length; i++)
                    {
                        myField[yCoordinate, xCoordinate + i] = shipClell;
                    }
                }
                else if(direction == DirectionTypes.Down)
                {


                    for (int i = 0; i < length; i++)
                    {
                        myField[yCoordinate + i, xCoordinate] = shipClell;
                    }
                }
                else if(direction == DirectionTypes.Left)
                {
                    //if(xCoordinate + 1 < length)
                    //{
                    //    return;
                    //}


                    for (int i = 0; i < length; i++)
                    {
                        myField[yCoordinate, xCoordinate - i] = shipClell;
                    }
                }
                isOkay = true;
            }

            

        }

        public static async Task ShowFields()
        {
            Console.WriteLine("Your ships: ");
            for(int x = 0; x < 10; x++)
            {
                for(int y = 0; y < 10; y++)
                {
                    Console.Write(myField[y,x]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }

            Console.WriteLine("Opponent ships: ");
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Console.Write(opponentField[y, x]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }
    }
}
