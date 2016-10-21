using NorwichCQRS.Core.EventMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Tests.Events
{
    public class FakeUserCreated : EventBase
    {
        public Guid UserGuid { get; set; }
        public string Username { get; set; }

        public FakeUserCreated(DateTime dateTime, Guid userGuid, string username):base(dateTime)
        {
            this.UserGuid = userGuid;
            this.Username = username;
        }
    }
}
