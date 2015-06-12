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
        private IDateTimeProvider _dateTimeProvider;
       
        private Nullable<DateTime> _dateTime;

        public DateTime DateTime
        {
            get { return _dateTime ?? _dateTimeProvider.CurrentDateTime; }
            set
            {
                _dateTime = value;
            }
        }

        public EventBase(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }
    }
}
