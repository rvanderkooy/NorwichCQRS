using NorwichCQRS.Infrastructure.EventMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Tests.Mocks
{
    public class MockEventBus : IEventBus
    {
        public Queue<IEvent> PublishedEvents { get; private set; }

        public MockEventBus()
        {
            this.PublishedEvents = new Queue<IEvent>();
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            this.PublishedEvents.Enqueue(@event);
        }
    }
}
