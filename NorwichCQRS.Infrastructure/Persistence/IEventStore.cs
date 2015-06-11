using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Infrastructure.Persistence
{
    public interface IEventStore
    {
        IEnumerable<IAggregateEvent> LoadAggregate(Guid aggregateGuid);
        void SaveChanges(IEnumerable<IAggregateEvent> uncommittedEvents);
    }
}
