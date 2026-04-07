using System;
using System.Collections;
using CCSWE.nanoFramework.Mediator;

namespace Emily.Clock.UnitTests.Mocks;

internal class MediatorMock: IMediator
{
    private readonly Hashtable _publishedEvents = new();
    private readonly Hashtable _subscribedEvents = new();

    private void DecrementSubscriberCount(Type eventType)
    {
        var eventName = eventType.FullName;
        if (!_subscribedEvents.Contains(eventName))
        {
            _subscribedEvents.Add(eventName, 0);
            return;
        }

        var count = (int)_subscribedEvents[eventType.FullName] - 1;

        _subscribedEvents[eventType.FullName] = count > 0 ? count : 0;
    }

    public int GetPublishedCount(Type eventType)
    {
        return (int)(_publishedEvents[eventType.FullName] ?? 0);
    }

    public int GetSubscriberCount(Type eventType)
    {
        return (int)(_subscribedEvents[eventType.FullName] ?? 0);
    }

    private void IncrementPublishedCount(Type eventType)
    {
        var eventName = eventType.FullName;
        if (!_publishedEvents.Contains(eventName))
        {
            _publishedEvents.Add(eventName, 1);
            return;
        }

        var count = (int)_publishedEvents[eventType.FullName] + 1;

        _publishedEvents[eventType.FullName] = count;
    }

    private void IncrementSubscriberCount(Type eventType)
    {
        var eventName = eventType.FullName;
        if (!_subscribedEvents.Contains(eventName))
        {
            _subscribedEvents.Add(eventName, 1);
            return;
        }

        var count = (int)_subscribedEvents[eventType.FullName] + 1;

        _subscribedEvents[eventType.FullName] = count;
    }

    public void Publish(IMediatorEvent mediatorEvent)
    {
        IncrementPublishedCount(mediatorEvent.GetType());
    }

    public void Subscribe(Type eventType, IMediatorEventHandler eventHandler)
    {
        IncrementSubscriberCount(eventType);
    }

    public void Subscribe(Type eventType, Type subscriberType)
    {
        IncrementSubscriberCount(eventType);
    }

    public void Unsubscribe(Type eventType, IMediatorEventHandler eventHandler)
    {
        DecrementSubscriberCount(eventType);
    }

    public void Unsubscribe(Type eventType, Type subscriberType)
    {
        DecrementSubscriberCount(eventType);
    }

    public bool WasEventPublished(Type eventType) => GetPublishedCount(eventType) > 0;

    public bool WasEventSubscribed(Type eventType) => GetSubscriberCount(eventType) > 0;
}