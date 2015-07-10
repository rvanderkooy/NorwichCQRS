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
            try
            {
                Trace.WriteLine(string.Format("Initializing EventSoucedAggregateRoot for AggregateGuid: {0}", aggregateGuid.ToString()));
                IEnumerable<IAggregateEvent> aggregateEventHistory = _eventStore.LoadAggregate(aggregateGuid);
                Trace.WriteLine(string.Format("Loaded {0} IAggregateEvents for AggregateGuid: {1}", aggregateEventHistory.Count(), aggregateGuid.ToString()));

                foreach (IAggregateEvent aggregateEvent in aggregateEventHistory)
                {
                    Trace.WriteLine(string.Format("Preparing to deserialize EventType: {0}", aggregateEvent.EventType));
                    Type eventType = FindType(aggregateEvent.EventType);

                    Trace.WriteLine(string.Format("Loaded type: {0}", eventType.FullName));

                    Trace.WriteLine(string.Format("Deserializing: {0}, for eventType: {1}", aggregateEvent.EventValue, eventType));
                    try
                    {
                        dynamic @event = JsonConvert.DeserializeObject(aggregateEvent.EventValue, eventType);
                        Trace.WriteLine("Applying Event");

                        this.Apply(@event, false);
                    }catch(Exception ex)
                    {
                        Trace.WriteLine(string.Format("Error occurred deserializing and applying event: {0} | {1} | {2}", ex.Message, ex.StackTrace, ex.InnerException));
                    }                    
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Error occurred in EventSourcedAggregateRoot.Initialize(aggregateGuid: {0})...{1}", aggregateGuid.ToString(), ex.ToString()));
            }
        }

        protected void Apply(IEvent @event, bool isNew)
        {
            Trace.WriteLine("In Apply(event: {0}, isNew: {1})");
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

            Trace.WriteLine("Calling When(event) for Aggregate");
            try
            {
                ((dynamic)this).When((dynamic)@event);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error occurred calling When(event): {0}", ex.ToString());
            }

            Trace.WriteLine("Apply completed");
        }

        protected void SaveAndPublishEvents()
        {
            _eventStore.SaveChanges(this.UncommittedEvents);

            this.UncommittedEvents.Clear();

            this.PublishEvents(_eventBus);
        }

        protected static Type FindType(string typeName)
        {
            Trace.WriteLine(string.Format("In FindType(typeName: {0})", typeName));

            if (_eventTypes.ContainsKey(typeName))
            {
                Trace.WriteLine(string.Format("Found key for {0}", typeName));
                return _eventTypes[typeName];
            }

            Trace.WriteLine(string.Format("Unable to find key for {0}", typeName));
            Type type = GetType(typeName);

            Trace.WriteLine(string.Format("Received Type: {0}...adding to EventTypes", typeName));
            _eventTypes.Add(typeName, type);

            Trace.WriteLine(string.Format("Returning Type: {0}", type.FullName));
            return type;
        }

        protected static Type GetType(string typeName)
        {
            Trace.WriteLine(string.Format("In GetType(typeName: {0})", typeName));
            try
            {
                Trace.WriteLine(string.Format("Attempting Type.GetType(typeName: {0})", typeName));
                var type = Type.GetType(typeName);

                if (type != null)
                {
                    Trace.WriteLine(string.Format("Loaded type: {0}...returning", type.FullName));
                    return type;
                }

                Trace.WriteLine("Iterating assemblies in AppDomain.CurrentDomain.GetAssemblies()");
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Trace.WriteLine(string.Format("Attempting a: {0}.GetType(typeName: {1})", a.FullName, typeName));
                    type = a.GetType(typeName);
                    if (type != null)
                    {
                        Trace.WriteLine(string.Format("Loaded type: {0}...in assembly: {1}...returning", type.FullName, a.FullName));
                        return type;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Error occurred in GetType(typeName: {0})...{1}", typeName, ex.ToString()));
            }
            return null;
        }
    }
}
