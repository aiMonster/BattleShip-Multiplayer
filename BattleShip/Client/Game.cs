using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Enums;
using Client.Extensions;
using Common.DTO;

namespace Client
{
    public static class Game
    {
        private const char freeCell = '~';
        private const char shipClell = 'O';
        private const char shotedCell = ' ';


        private static char[,] myField = new char[10,10];
        private static char[,] opponentField = new char[10, 10];

        static Game()
        {           
            myField.Fill(freeCell);
            opponentField.Fill(freeCell);
            //await ShowFields();
        }

        public static async Task<ShotResultTypes> CheckShot(MoveCoordinates coordinates)
        {
            if(myField[coordinates.X, coordinates.Y] == freeCell)
            {
                return ShotResultTypes.MovePast;
            }            
            else if(myField[coordinates.X, coordinates.Y] == shipClell)
            {
                return ShotResultTypes.MoveInjured;
            }
            return ShotResultTypes.MovePast;
        }

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

        private static async Task PutShip(int length)
        {
            bool isOkay = false;
            while(!isOkay)
            {
                Console.WriteLine("Ener X coordinate:");
                int xCoordinate = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Ener Y coordinate:");
                int yCoordinate = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Ener direction:");
                DirectionTypes direction = (DirectionTypes)Convert.ToInt32(Console.ReadLine());

                if(direction == DirectionTypes.Up)
                {


                    for(int i = 0; i < length; i++)
                    {
                        myField[xCoordinate, yCoordinate - i] = shipClell;
                    }
                }
                else if(direction == DirectionTypes.Right)
                {


                    for (int i = 0; i < length; i++)
                    {
                        myField[xCoordinate + i, yCoordinate] = shipClell;
                    }
                }
                else if(direction == DirectionTypes.Down)
                {


                    for (int i = 0; i < length; i++)
                    {
                        myField[xCoordinate, yCoordinate + i] = shipClell;
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
                        myField[xCoordinate - i, yCoordinate] = shipClell;
                    }
                }
                isOkay = true;
            }

            

        }

        public static async Task ShowFields()
        {
            for(int x = 0; x < 10; x++)
            {
                for(int y = 0; y < 10; y++)
                {
                    Console.Write(myField[x,y]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }
    }
}
