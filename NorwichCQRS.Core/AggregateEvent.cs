using NorwichCQRS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Core
{
    public class AggregateEvent:IAggregateEvent
    {
        public int AggregateEventID { get; set; }
        public Guid AggregateEventGuid { get; set; }
        public Guid AggregateGuid { get; set; }
        public string EventType { get; set; }
        public string EventValue { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
