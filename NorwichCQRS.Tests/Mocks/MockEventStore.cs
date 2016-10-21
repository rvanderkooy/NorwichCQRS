using NorwichCQRS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorwichCQRS.Infrastructure;

namespace NorwichCQRS.Tests.Mocks
{
    public class MockEventStore : IEventStore
    {
        private List<IAggregateEvent> _aggregateEventsMockStore;

        public MockEventStore(List<IAggregateEvent> aggregateEventsMockStore)
        {
            _aggregateEventsMockStore = aggregateEventsMockStore;
        }

        public IEnumerable<IAggregateEvent> LoadAggregate(Guid aggregateGuid)
        {
            return _aggregateEventsMockStore.Where(a => a.AggregateGuid == aggregateGuid).ToList();
        }

        public void SaveChanges(Type aggregateType, IEnumerable<IAggregateEvent> uncommittedEvents)
        {
            _aggregateEventsMockStore.AddRange(uncommittedEvents);
        }

        public IEnumerable<IAggregateEvent> GetEventsMockStore()
        {
            return _aggregateEventsMockStore;
        }
    }
}
