﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Enums
{
    public enum NotificationTypes
    {     
        //to server
        RandomGameRequest,//
        CreateGame,//
        ParticipateGame,//
        ShipsPlaced,//       

        //to both
        MoveMade,//to server - done// to client - done
        MoveChecked,//to server - done// to client - done

        //to client                
        GameCreated,//
        GameNotFound,//
        WaitingForOpponentShips,//
        GamePasswordNotValid,//
        WatingForShips,//       
        OpponentSurrended,//
        Won,//
        Lost,//
        YourMove,//
        OpponentMove//

        
    }
}
