using NorwichCQRS.Core.CommandMessaging;
using NorwichCQRS.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Tests.Commands
{
    public class CreateFakeUser : Command
    {
        public Guid UserGuid { get; set; }
        public string Username { get; set; }

        public CreateFakeUser(IDateTimeProvider dateTimeProvider, Guid userGuid, string username) : base(dateTimeProvider)
        {
            this.UserGuid = userGuid;
            this.Username = username;
        }
    }
}
