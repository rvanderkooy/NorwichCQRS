using NorwichCQRS.Infrastructure.EventMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Core.EventMessaging
{
    public class InMemoryEventBus : IEventBus
    {
        private IEventHandlerFactory _eventHandlerFactory;

        public InMemoryEventBus(IEventHandlerFactory eventHandlerFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            IEnumerable<IEventHandler<TEvent>> eventHandlers = _eventHandlerFactory.GetHandlers<TEvent>();

            foreach (IEventHandler<TEvent> eventHandler in eventHandlers)
            {
                eventHandler.Handle(@event);
            }
        }
    }
}
