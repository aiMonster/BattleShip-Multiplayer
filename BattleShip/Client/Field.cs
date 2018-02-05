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
    public static class Field
    {
        private static Ship deck_4 = new Ship(4);
        private static Ship[] deck_3 = new Ship[2] { new Ship(3), new Ship(3) };
        private static Ship[] deck_2 = new Ship[3] { new Ship(2), new Ship(2), new Ship(2) };
        private static Ship[] deck_1 = new Ship[4] { new Ship(1), new Ship(1), new Ship(1), new Ship(1) };

        private const char waterCell = '~';
        private const char shipCell = 'O';
        private const char emptyCell = ' ';
        private const char deadShipCell = 'X';

        private static char[,] myField = new char[18,18];
        private static char[,] opponentField = new char[18, 18];
               

        //will work but we don't check is it killed
        public static async Task<ShotResultTypes> CheckShot(MoveCoordinates coordinates)
        {
            if(myField[coordinates.A, coordinates.B] == waterCell)
            {
                myField[coordinates.A, coordinates.B] = emptyCell;
                return ShotResultTypes.MovePast;
            }            
            else if(myField[coordinates.A, coordinates.B] == shipCell)
            {
                myField[coordinates.A, coordinates.B] = deadShipCell;
                bool wereWehere = false;
                ShotResultTypes response = ShotResultTypes.MoveInjured;

                int aliveShips = 0;
                int aliveCells = 0;
                //checking 4 deck ship
                for (int i = 0; i < 4; i++)
                {                   
                   if (deck_4.cells[i].A == coordinates.A && deck_4.cells[i].B == coordinates.B)
                   {
                        deck_4.cells[i].cellState = CellStateTypes.Injured;
                        wereWehere = true;
                   }
                   if(deck_4.cells[i].cellState == CellStateTypes.Alive)
                   {
                        aliveCells++;
                   }
                }
                if(aliveCells != 0)
                {
                    aliveShips++;
                }
                else if(wereWehere)
                {
                    response = ShotResultTypes.MoveKilled;
                    wereWehere = false;
                }                
                aliveCells = 0;

                //checking 3 deck ships
                for(int i = 0; i < 2; i++)
                {
                    for(int c = 0; c < 3; c++)
                    {
                        if(deck_3[i].cells[c].A == coordinates.A && deck_3[i].cells[c].B == coordinates.B)
                        {
                            deck_3[i].cells[c].cellState = CellStateTypes.Injured;
                            wereWehere = true;
                        }
                        if(deck_3[i].cells[c].cellState == CellStateTypes.Alive)
                        {
                            aliveCells++;
                        }
                    }

                    if (aliveCells != 0)
                    {
                        aliveShips++;
                    }
                    else if (wereWehere)
                    {
                        response = ShotResultTypes.MoveKilled;
                        wereWehere = false;
                    }
                    aliveCells = 0;
                }
               

                //checking 2 deck ships
                for (int i = 0; i < 3; i++)
                {
                    for (int c = 0; c < 2; c++)
                    {
                        if (deck_2[i].cells[c].A == coordinates.A && deck_2[i].cells[c].B == coordinates.B)
                        {
                            deck_2[i].cells[c].cellState = CellStateTypes.Injured;
                            wereWehere = true;
                        }
                        if (deck_2[i].cells[c].cellState == CellStateTypes.Alive)
                        {
                            aliveCells++;
                        }
                    }

                    if (aliveCells != 0)
                    {
                        aliveShips++;
                    }
                    else if (wereWehere)
                    {
                        response = ShotResultTypes.MoveKilled;
                        wereWehere = false;
                    }
                    aliveCells = 0;
                }
                
                
                //checking 1 deck ships
                for (int i = 0; i < 4; i++)
                {
                    
                    if (deck_1[i].cells[0].A == coordinates.A && deck_1[i].cells[0].B == coordinates.B)
                    {
                        deck_1[i].cells[0].cellState = CellStateTypes.Injured;
                        wereWehere = true;
                    }
                    if (deck_1[i].cells[0].cellState == CellStateTypes.Alive)
                    {
                        aliveCells++;
                    }

                    if (aliveCells != 0)
                    {
                        aliveShips++;
                    }
                    else if (wereWehere)
                    {
                        response = ShotResultTypes.MoveKilled;
                        wereWehere = false;
                    }
                    aliveCells = 0;
                }
                

                myField[coordinates.A, coordinates.B] = deadShipCell;
                if(aliveShips == 0)
                {
                    return ShotResultTypes.MoveFatal;
                }
                else
                {
                    return response;
                }
                
            }
           
            return ShotResultTypes.MovePast;
        }

        #region C++ logic
        //works only by yourself
        //code from C++
        public static async Task FillField()
        {
            //reseting fields
            myField.Fill(shipCell, waterCell);
            opponentField.Fill(shipCell, waterCell);
            ShowFields();
            FillingFieldTypes fillingType = FillingFieldTypes.Random;

            Console.WriteLine("Do you want to fill field by yourself(1) or by random(2)? -");
            int answer = Convert.ToInt32(Console.ReadLine());
            switch(answer)
            {
                case 1:
                    fillingType = FillingFieldTypes.Yourself;
                    break;
                case 2:
                    fillingType = FillingFieldTypes.Random;
                    break;
            }

            bool isPutOk = true;
            (DirectionTypes direction, int a, int b) coordinatesAnswer = (DirectionTypes.Down, 0,0);


            //ставимо 1 корабель довжиною 4
            do//робити доти, поки корабель не буде виставлений корректно
            {
                isPutOk = false;
                if (fillingType == FillingFieldTypes.Yourself)
                {
                    coordinatesAnswer = TakeCoordinatesWithDirection();
                }
                else
                {
                    Random rnd = new Random();
                    coordinatesAnswer.a = rnd.Next(4, 13);
                    coordinatesAnswer.b = rnd.Next(4, 13);
                    coordinatesAnswer.direction = (DirectionTypes)rnd.Next(0, 3);
                }
                //пробуємо поставити
                isPutOk = PutShip((coordinatesAnswer.direction, 4, coordinatesAnswer.a, coordinatesAnswer.b), 0);
            }
            while (!isPutOk); //роби лоти, доки вдало та правильно не розмістимо корабель
            
            //для корабля довжиною 3, їх має бути 2
            for (int q = 0; q < 2; q++)
            {
                do//робити доти, поки корабель не буде виставлений корректно
                {
                    isPutOk = false;
                    if (fillingType == FillingFieldTypes.Yourself)
                    {
                        coordinatesAnswer = TakeCoordinatesWithDirection();
                    }
                    else
                    {
                        Random rnd = new Random();
                        coordinatesAnswer.a = rnd.Next(4, 13);
                        coordinatesAnswer.b = rnd.Next(4, 13);
                        coordinatesAnswer.direction = (DirectionTypes)rnd.Next(0, 3);
                    }
                    isPutOk = PutShip((coordinatesAnswer.direction, 3, coordinatesAnswer.a, coordinatesAnswer.b), q);
                    
                }
                while (!isPutOk);
            }

            //для 3 кораблів довжиною 2
            for (int q = 0; q < 3; q++)
            {
                do//робити доти, поки корабель не буде виставлений корректно
                {
                    isPutOk = false;
                    if (fillingType == FillingFieldTypes.Yourself)
                    {
                        coordinatesAnswer = TakeCoordinatesWithDirection();
                    }
                    else
                    {
                        Random rnd = new Random();
                        coordinatesAnswer.a = rnd.Next(4, 13);
                        coordinatesAnswer.b = rnd.Next(4, 13);
                        coordinatesAnswer.direction = (DirectionTypes)rnd.Next(0, 3);
                    }
                    isPutOk = PutShip((coordinatesAnswer.direction, 2, coordinatesAnswer.a, coordinatesAnswer.b), q);
                   
                }
                while (!isPutOk);
            }

            //для 4 кораблів довжиною 1
            for (int q = 0; q < 4; q++)
            {
                do//робити доти, поки корабель не буде виставлений корректно
                {
                    isPutOk = false;
                    if (fillingType == FillingFieldTypes.Yourself)
                    {
                        coordinatesAnswer = TakeCoordinatesWithDirection(false);
                    }
                    else
                    {
                        Random rnd = new Random();
                        coordinatesAnswer.a = rnd.Next(4, 13);
                        coordinatesAnswer.b = rnd.Next(4, 13);
                        coordinatesAnswer.direction = (DirectionTypes)rnd.Next(0, 3);                        
                    }
                    isPutOk = PutShip((coordinatesAnswer.direction, 1, coordinatesAnswer.a, coordinatesAnswer.b), q);                    ;
                }
                while (!isPutOk);
            }

        }

        //code from C++
        private static bool PutShip((DirectionTypes direction, int length, int a, int b) shot, int number)
        {
            bool temp = false;
            int f = shot.a;
            int s = shot.b;

            // якщо напрямок 1
            if (shot.direction == DirectionTypes.Left)
            {
                if (CheckRotation(shot))
                {
                    for (int a = 0; a < shot.length; a++)
                    {
                        myField[f,s - a] = shipCell;                        
                        switch (shot.length)
                        {
                            case 1: deck_1[number].SetCoordinate(a, f, s -a); break;
                            case 2: deck_2[number].SetCoordinate(a, f, s - a); break;
                            case 3: deck_3[number].SetCoordinate(a, f, s - a); break;
                            case 4: deck_4.SetCoordinate(a, f, s - a); break;
                        }
                    }
                    temp = true;
                }
            }


            //якщо напрямок 2
            else if (shot.direction == DirectionTypes.Down)
            {
                if (CheckRotation(shot))
                {
                    for (int a = 0; a < shot.length; a++)
                    {
                        myField[f + a,s] = shipCell;                        
                        switch (shot.length)
                        {
                            case 1: deck_1[number].SetCoordinate(a, f + a, s); break;
                            case 2: deck_2[number].SetCoordinate(a, f + a, s); break;
                            case 3: deck_3[number].SetCoordinate(a, f + a, s); break;
                            case 4: deck_4.SetCoordinate(a, f + a, s); break;
                        }
                    }
                    temp = true;
                }
            }


            //якщо напрямок 3
            else if (shot.direction == DirectionTypes.Right)
            {
                if (CheckRotation(shot))
                {
                    for (int a = 0; a < shot.length; a++)
                    {
                        myField[f,s + a] = shipCell;                        
                        switch (shot.length)
                        {
                            case 1: deck_1[number].SetCoordinate(a, f, s + a); break;
                            case 2: deck_2[number].SetCoordinate(a, f, s + a); break;
                            case 3: deck_3[number].SetCoordinate(a, f, s + a); break;
                            case 4: deck_4.SetCoordinate(a, f, s + a); break;
                        }
                    }
                    temp = true;
                }
            }

            //якщо напрямок 4
            else if (shot.direction == DirectionTypes.Up)
            {
                if (CheckRotation(shot))
                {
                    for (int a = 0; a < shot.length; a++)
                    {
                        myField[f - a,s] = shipCell;                        
                        switch (shot.length)
                        {
                            case 1: deck_1[number].SetCoordinate(a, f - a, s); break;
                            case 2: deck_2[number].SetCoordinate(a, f - a, s); break;
                            case 3: deck_3[number].SetCoordinate(a, f - a, s); break;
                            case 4: deck_4.SetCoordinate(a, f - a, s); break;
                        }
                    }
                    temp = true;
                }
            }

            return temp; //1 - корабель успішно поставлено, 0 - потрібно повторити
        }

        //code from C++
        private static bool CheckRotation((DirectionTypes direction, int length, int a, int b) shot)
        {
            bool temp; // якщо 1 то можна, якщо 0 то ні
            //int d = shot.direction;
            int f = shot.a; //f - first coordinate
            int s = shot.b; //s - second coordinate

            //перевіряємо в залежності від напрямку
            // напрямок 1(Вліво)
            if (shot.direction == DirectionTypes.Left)
            {
                temp = true;
                for (int a = f - 1; a <= f + 1; a++)
                {
                    for (int b = s - shot.length; b <= s + 1; b++)
                    {
                        if (myField[a,b] != waterCell)
                        {
                            temp = false; // якщо якись елемент не дорівнює вільному положенні то корабель вже там бути не може
                        }
                    }
                }
                return temp; //якщо корабель можна ставити повертаєм 1
            }

            //напрямок 2
            else if (shot.direction == DirectionTypes.Down)
            {
                temp = true;
                for (int a = f - 1; a <= f + shot.length; a++)
                {
                    for (int b = s - 1; b <= s + 1; b++)
                    {
                        if (myField[a, b] != waterCell)
                        {
                            temp = false;
                        }
                    }
                }
                return temp;
            }

            //напрямок 3
            else if (shot.direction == DirectionTypes.Right)
            {
                temp = true;
                for (int a = f - 1; a <= f + 1; a++)
                {
                    for (int b = s - 1; b <= s + shot.length; b++)
                    {
                        if (myField[a, b] != waterCell)
                        {
                            temp = false;
                        }
                    }
                }
                return temp;
            }

            //напрямок 4
            else if (shot.direction == DirectionTypes.Up)
            {
                temp = true;
                for (int a = f - shot.length; a <= f + 1; a++)
                {
                    for (int b = s - 1; b <= s + 1; b++)
                    {
                        if (myField[a, b] != waterCell)
                        {
                            temp = false;
                        }
                    }
                }

                return temp;
            }
            return false;
        }
        

        private static (DirectionTypes, int, int) TakeCoordinatesWithDirection(bool isItMoreThanOne = true)
        {
            Console.WriteLine(new string('=', 20));
            ShowMyField();

            Console.WriteLine("Ener a coordinate(0-9):");
            int aCoordinate = Convert.ToInt32(Console.ReadLine()) + 4;

            Console.WriteLine("Ener b coordinate(0-9(qwertyuiop)):");
            int bCoordinate = Convert.ToInt32(Console.ReadLine()) + 4;

            DirectionTypes direction = DirectionTypes.Down;
            if(isItMoreThanOne)
            {
                Console.WriteLine("Enter direction:");
                direction = (DirectionTypes)Convert.ToInt32(Console.ReadLine());
            }
            return (direction, aCoordinate, bCoordinate);
        }
        #endregion


        //done but we don't surround when it is killed
        public static void MarkOurShot(ShotResult shot)
        {
            switch (shot.shotResult)
            {
                case ShotResultTypes.MoveInjured:
                    Console.WriteLine("You just injured me");
                    goto case ShotResultTypes.MoveFatal;
                case ShotResultTypes.MoveKilled:
                    Console.WriteLine("You just killed me");
                    goto case ShotResultTypes.MoveFatal;
                case ShotResultTypes.MoveFatal:
                    opponentField[shot.A, shot.B] = deadShipCell;
                    break;
                case ShotResultTypes.MovePast:
                    Console.WriteLine("Your move was past by");
                    opponentField[shot.A, shot.B] = waterCell;
                    break;
            }

        }
               
        public static void ShowMyField()
        {
            for (int a = 4; a < 14; a++)
            {
                for (int b = 4; b < 14; b++)
                {
                    Console.Write(myField[a, b]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        public static void ShowFields()
        {
            Console.WriteLine("Your ships: ");
            for(int a = 0; a < 18; a++)
            {
                for(int b = 0; b < 18; b++)
                {
                    Console.Write(myField[a,b]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }

            Console.WriteLine("Opponent ships: ");
            for (int a = 0; a < 18; a++)
            {
                for (int b = 0; b < 18; b++)
                {
                    Console.Write(opponentField[a, b]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }
    }
}
