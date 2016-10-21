using NorwichCQRS.Core;
using NorwichCQRS.Infrastructure.EventMessaging;
using NorwichCQRS.Infrastructure.Persistence;
using NorwichCQRS.Infrastructure.Providers;
using NorwichCQRS.Tests.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Tests
{
    public class FakeUser : EventSourcedAggregateRoot
    {        
        public string Username { get; private set; }

        protected FakeUser(Guid userGuid, IEventStore eventStore, IEventBus eventBus, IDateTimeProvider dateTimeProvider)
            : base(userGuid, eventStore, eventBus, dateTimeProvider)
        {
        }

        public static FakeUser Retrieve(Guid userGuid, IEventStore eventStore, IEventBus eventBus, IDateTimeProvider dateTimeProvider)
        {
            // Load the FakeUser, the events will be loaded in the base constructor
            FakeUser fakeUser = new FakeUser(userGuid, eventStore, eventBus, dateTimeProvider);

            return fakeUser;
        }

        public static FakeUser CreateNew(Guid userGuid, IEventStore eventStore, IEventBus eventBus, IDateTimeProvider dateTimeProvider, DateTime createdDate, string username)
        {
            // Instantiate the new FakeUser
            FakeUser fakeUser = new FakeUser(userGuid, eventStore, eventBus, dateTimeProvider);

            // Create the event
            FakeUserCreated @event = new FakeUserCreated(createdDate, userGuid, username);

            // Apply the event
            fakeUser.Apply(@event, true);

            // Save the events and publish them
            fakeUser.SaveAndPublishEvents();

            return fakeUser;            
        }


        #region Apply Events

        public void When(FakeUserCreated @event)
        {
            this.Username = @event.Username;
        }

        #endregion
    }
}
