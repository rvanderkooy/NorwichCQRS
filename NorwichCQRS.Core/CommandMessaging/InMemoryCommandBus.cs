using NorwichCQRS.Infrastructure.CommandMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Core.CommandMessaging
{
    public class InMemoryCommandBus : ICommandBus
    {
        private ICommandHandlerFactory _commandHandlerFactory;

        public InMemoryCommandBus(ICommandHandlerFactory commandHandlerFactory)
        {
            _commandHandlerFactory = commandHandlerFactory;
        }

        public void Send<TCommand>(TCommand command)
        {
            ICommandHandler<TCommand> commandHandler = _commandHandlerFactory.GetHandler<TCommand>();

            commandHandler.Process(command);
        }


        public void Subscribe<TCommand>(Action<TCommand> handler) where TCommand : class
        {
            throw new NotImplementedException();
        }
    }
}
