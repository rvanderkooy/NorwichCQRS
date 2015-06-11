using NorwichCQRS.Infrastructure.CommandMessaging;
using NorwichCQRS.Infrastructure.EventMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Core.CommandMessaging
{
    public class CommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        protected IEventBus EventBus { get; set; }

        public CommandHandler(IEventBus eventBus)
        {
            if (eventBus == null)
            {
                throw new ArgumentException("EventBus cannot be null.");
            }

            this.EventBus = eventBus;
        }

        public virtual void Process(TCommand command)
        {

        }        
    }
}
