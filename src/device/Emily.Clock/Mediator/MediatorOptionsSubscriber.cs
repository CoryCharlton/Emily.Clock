using System;

namespace Emily.Clock.Mediator
{
    // TODO: Use library version once it's public
    public class MediatorOptionsSubscriber
    {
        public Type EventType { get; }
        public Type SubscriberType { get; }

        internal MediatorOptionsSubscriber(Type eventType, Type subscriberType)
        {
            EventType = eventType;
            SubscriberType = subscriberType;
        }
    }
}
