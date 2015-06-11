using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Infrastructure
{
    public interface IAggregateEvent
    {
        int AggregateEventID { get; set; }
        Guid AggregateEventGuid { get; set; }
        Guid AggregateGuid { get; set; }
        string EventType { get; set; }
        string EventValue { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
