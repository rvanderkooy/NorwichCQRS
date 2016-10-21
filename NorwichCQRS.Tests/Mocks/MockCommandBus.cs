using NorwichCQRS.Infrastructure.CommandMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Tests.Mocks
{
    public class MockCommandBus : ICommandBus
    {
        public Queue<object> SentCommands { get; private set; }

        public MockCommandBus()
        {
            this.SentCommands = new Queue<object>();
        }

        public void Send<TCommand>(TCommand command)
        {
            this.SentCommands.Enqueue(command);    
        }
    }
}
