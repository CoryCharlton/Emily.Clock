using System;
using System.Collections;

namespace Emily.Clock.Mediator
{
    // TODO: Use library version once it's public
    public class MediatorOptions
    {
        public ArrayList Subscribers { get; } = new();

        /// <summary>
        /// Adds subscriber (event handler) to an event.
        /// </summary>
        /// <param name="eventType">Type of the event. The event must implement IEvent interface.</param>
        /// <param name="subscriberType">Type of the subscriber (as registered in DI). The subscriber must implement IEventHandler interface.</param>
        public void AddSubscriber(Type eventType, Type subscriberType)
        {
            Subscribers.Add(new MediatorOptionsSubscriber(eventType, subscriberType));
        }
    }
}
