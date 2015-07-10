using Newtonsoft.Json;
using NorwichCQRS.Infrastructure;
using NorwichCQRS.Infrastructure.EventMessaging;
using NorwichCQRS.Infrastructure.Persistence;
using NorwichCQRS.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Core
{
    public class EventSourcedAggregateRoot : AggregateRoot, IEventSourcedAggregateRoot
    {
        public Guid AggregateGuid { get; internal set; }
        private IEventStore _eventStore;
        private IEventBus _eventBus;

        protected static Dictionary<string, Type> _eventTypes = new Dictionary<string, Type>();

        protected List<AggregateEvent> UncommittedEvents { get; set; }

        public EventSourcedAggregateRoot(Guid aggregateGuid, IEventStore eventStore, IEventBus eventBus, IDateTimeProvider dateTimeProvider)
        {
            if (eventStore == null)
            {
                throw new ArgumentNullException("EventStore cannot be null.");
            }
            if (eventBus == null)
            {
                throw new ArgumentNullException("EventBus cannot be null.");
            }

            _eventStore = eventStore;
            _eventBus = eventBus;

            this.AggregateGuid = aggregateGuid;

            this.UncommittedEvents = new List<AggregateEvent>();

            this.Initialize(aggregateGuid);
        }



        protected void Initialize(Guid aggregateGuid)
        {
            Trace.WriteLine(string.Format("Initializing EventSoucedAggregateRoot for AggregateGuid: {0}", aggregateGuid.ToString()));
            try
            {
                IEnumerable<IAggregateEvent> aggregateEventHistory = _eventStore.LoadAggregate(aggregateGuid);

                foreach (IAggregateEvent aggregateEvent in aggregateEventHistory)
                {
                    Type eventType = FindType(aggregateEvent.EventType);

                    dynamic @event = JsonConvert.DeserializeObject(aggregateEvent.EventValue, eventType);

                    this.Apply(@event, false);
                }
            }catch(Exception ex)
            {
                Trace.WriteLine(string.Format("Error occurred in EventSourcedAggregateRoot.Initialize(aggregateGuid: {0})...{1}", aggregateGuid.ToString(), ex.ToString()));
            }
        }

        protected void Apply(IEvent @event, bool isNew)
        {
            if (isNew)
            {
                this.QueueEvent<IEvent>(@event);

                AggregateEvent aggregateEvent = new AggregateEvent();
                aggregateEvent.AggregateEventGuid = Guid.NewGuid();
                aggregateEvent.AggregateGuid = this.AggregateGuid;
                aggregateEvent.CreatedDate = @event.DateTime;
                //aggregateEvent.EventType = @event.GetType().AssemblyQualifiedName;
                aggregateEvent.EventType = @event.GetType().FullName;
                aggregateEvent.EventValue = JsonConvert.SerializeObject(@event);

                this.UncommittedEvents.Add(aggregateEvent);
            }

            ((dynamic)this).When((dynamic)@event);
        }

        protected void SaveAndPublishEvents()
        {
            _eventStore.SaveChanges(this.UncommittedEvents);

            this.UncommittedEvents.Clear();

            this.PublishEvents(_eventBus);
        }

        protected static Type FindType(string typeName)
        {
            if (_eventTypes.ContainsKey(typeName))
            {
                return _eventTypes[typeName];
            }

            Type type = GetType(typeName);

            _eventTypes.Add(typeName, type);

            return type;
        }

        protected static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}
