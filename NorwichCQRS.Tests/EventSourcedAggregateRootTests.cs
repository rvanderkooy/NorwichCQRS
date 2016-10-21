using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorwichCQRS.Infrastructure.EventMessaging;
using NorwichCQRS.Tests.Mocks;
using NorwichCQRS.Infrastructure.Persistence;
using NorwichCQRS.Infrastructure;
using System.Collections.Generic;
using NorwichCQRS.Infrastructure.Providers;
using NorwichCQRS.Core.Providers;
using NorwichCQRS.Tests.Events;

namespace NorwichCQRS.Tests
{
    [TestClass]
    public class EventSourcedAggregateRootTests
    {
        [TestMethod]
        public void WhenAFakeUserIsCreated_TheAggregateGuidIsSet()
        {
            // Arrange
            IEventStore eventStore = new MockEventStore(new List<IAggregateEvent>());
            IEventBus eventBus = new MockEventBus();
            IDateTimeProvider dateTimeProvider = new DateTimeProvider();
            Guid userGuid = Guid.NewGuid();
            string username = "FakeUser";

            // Act
            FakeUser fakeUser = FakeUser.CreateNew(userGuid, eventStore, eventBus, dateTimeProvider, dateTimeProvider.CurrentDateTime, username);

            // Assert
            Assert.AreEqual(userGuid, fakeUser.AggregateGuid);
        }

        [TestMethod]
        public void WhenAFakeUserIsCreated_TheUsernameIsSet()
        {
            // Arrange
            IEventStore eventStore = new MockEventStore(new List<IAggregateEvent>());
            IEventBus eventBus = new MockEventBus();
            IDateTimeProvider dateTimeProvider = new DateTimeProvider();
            Guid userGuid = Guid.NewGuid();
            string username = "FakeUser";

            // Act
            FakeUser fakeUser = FakeUser.CreateNew(userGuid, eventStore, eventBus, dateTimeProvider, dateTimeProvider.CurrentDateTime, username);

            // Assert
            Assert.AreEqual("FakeUser", fakeUser.Username);
        }

        [TestMethod]
        public void WhenAFakeUserIsCreated_TheVersionIsOne()
        {
            // Arrange
            IEventStore eventStore = new MockEventStore(new List<IAggregateEvent>());
            IEventBus eventBus = new MockEventBus();
            IDateTimeProvider dateTimeProvider = new DateTimeProvider();
            Guid userGuid = Guid.NewGuid();
            string username = "FakeUser";

            // Act
            FakeUser fakeUser = FakeUser.CreateNew(userGuid, eventStore, eventBus, dateTimeProvider, dateTimeProvider.CurrentDateTime, username);

            // Assert
            Assert.AreEqual(1, fakeUser.Version);
        }

        [TestMethod]
        public void WhenAFakeUserIsCreated_TheEventIsPublishedToTheEventBus()
        {
            // Arrange
            IEventStore eventStore = new MockEventStore(new List<IAggregateEvent>());
            MockEventBus eventBus = new MockEventBus();
            IDateTimeProvider dateTimeProvider = new DateTimeProvider();
            Guid userGuid = Guid.NewGuid();
            string username = "FakeUser";

            // Act
            FakeUser fakeUser = FakeUser.CreateNew(userGuid, eventStore, eventBus, dateTimeProvider, dateTimeProvider.CurrentDateTime, username);

            // Assert
            Assert.IsTrue(eventBus.PublishedEvents.Count() >= 1);

            IEvent @event = eventBus.PublishedEvents.Dequeue();
            Assert.IsInstanceOfType(@event, typeof(FakeUserCreated));            
        }
    }
}
