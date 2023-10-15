using MakoIoT.Device.Services.Mediator;
using System;
using System.Collections;
using System.Threading;

#nullable enable
namespace Emily.Clock.Mediator
{
    /// <summary>
    /// Threaded implementation of Mediator pattern
    /// </summary>
    public class AsyncMediator : IMediator
    {
        private readonly object _eventLock = new();
        private readonly Queue _eventQueue = new();
        private readonly AutoResetEvent _eventWaiting = new(false);
        private readonly IServiceProvider _serviceProvider;
        private readonly Hashtable _subscribers = new();
        private readonly Hashtable _subscriberTypes = new();

        public AsyncMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            new Thread(PublishThread).Start();
        }

        public AsyncMediator(MediatorOptions options, IServiceProvider serviceProvider): this(serviceProvider)
        {
            foreach (MediatorOptionsSubscriber subscriber in options.Subscribers)
            {
                Subscribe(subscriber.EventType, subscriber.SubscriberType);
            }
        }

        private IEvent? DequeueEvent()
        {
            lock (_eventLock)
            {
                return _eventQueue.Count > 0 ? _eventQueue.Dequeue() as IEvent : null;
            }
        }

        /// <inheritdoc />
        public void Publish(IEvent @event)
        {
            lock (_eventLock)
            {
                _eventQueue.Enqueue(@event);
                _eventWaiting.Set();
            }
        }

        public void PublishInternal(IEvent @event)
        {
            var eventName = @event.GetType().FullName;

            if (_subscribers.Contains(eventName))
            {
                foreach (IEventHandler subscriber in (ArrayList)_subscribers[eventName])
                {
                    subscriber.Handle(@event);
                }
            }

            if (_subscriberTypes.Contains(eventName))
            {
                foreach (Type subscriberType in (ArrayList)_subscriberTypes[eventName])
                {
                    ((IEventHandler)_serviceProvider.GetService(subscriberType)).Handle(@event);
                }
            }
        }

        private void PublishThread()
        {
            while (true)
            {
                var eventToPublish = DequeueEvent();

                while (eventToPublish is not null)
                {
                    PublishInternal(eventToPublish);

                    eventToPublish = DequeueEvent();
                }

                _eventWaiting.WaitOne();
            }
        }

        /// <inheritdoc />
        public void Subscribe(Type eventType, IEventHandler subscriber)
        {
            var eventName = eventType.FullName;
            if (!_subscribers.Contains(eventName))
            {
                _subscribers.Add(eventName, new ArrayList { subscriber });
                return;
            }

            var subscribers = (ArrayList)_subscribers[eventName];
            if (!subscribers.Contains(subscriber))
            {
                subscribers.Add(subscriber);
            }
        }

        /// <inheritdoc />
        public void Subscribe(Type eventType, Type subscriberType)
        {
            var eventName = eventType.FullName;
            if (!_subscriberTypes.Contains(eventName))
            {
                _subscriberTypes.Add(eventName, new ArrayList { subscriberType });
                return;
            }

            var subscribers = (ArrayList)_subscriberTypes[eventName];
            if (!subscribers.Contains(subscriberType))
            {
                subscribers.Add(subscriberType);
            }
        }

        /// <inheritdoc />
        public void Unsubscribe(Type eventType, IEventHandler subscriber)
        {
            var eventName = eventType.FullName;
            if (!_subscribers.Contains(eventName))
            {
                return;
            }

            var subscribers = (ArrayList)_subscribers[eventName];
            if (subscribers.Contains(subscriber))
            {
                subscribers.Remove(subscriber);
            }
        }

        /// <inheritdoc />
        public void Unsubscribe(Type eventType, Type subscriberType)
        {
            var eventName = eventType.FullName;
            if (!_subscriberTypes.Contains(eventName))
            {
                return;
            }

            var subscribers = (ArrayList)_subscriberTypes[eventName];
            if (subscribers.Contains(subscriberType))
            {
                subscribers.Remove(subscriberType);
            }
        }
    }
}
