﻿using NorwichCQRS.Infrastructure;
using NorwichCQRS.Infrastructure.EventMessaging;
using NorwichCQRS.Infrastructure.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Core
{
    public class AggregateRoot : IAggregateRoot
    {
        private Queue EventQueue { get; set; }

        public AggregateRoot()
        {
            this.EventQueue = new Queue();
        }

        public void PublishEvents(IEventBus eventBus)
        {
            while (this.EventQueue.Count > 0)
            {
                dynamic @event = this.EventQueue.Dequeue();

                eventBus.Publish(@event);
            }
        }

        protected void QueueEvent<TEvent>(IEvent @event) where TEvent : IEvent
        {
            this.EventQueue.Enqueue(@event);
        }
    }
}
