using NorwichCQRS.Infrastructure.EventMessaging;
using NorwichCQRS.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Core.EventMessaging
{
    public class EventBase : IEvent
    {               
        public DateTime DateTime { get; set; }       
 
        public EventBase(DateTime dateTime)
        {
            if (dateTime != default(DateTime))
            {
                this.DateTime = dateTime;
            }
        }
    }
}
