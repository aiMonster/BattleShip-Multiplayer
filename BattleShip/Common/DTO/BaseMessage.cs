using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    public class BaseMessage
    {
        public NotificationTypes Type { get; set; }

        public BaseMessage() { }

        public BaseMessage(NotificationTypes type)
        {
            Type = type;
        }
    }
}
