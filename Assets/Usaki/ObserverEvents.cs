using System;
using System.Collections.Generic;

public class ObserverEvents<EventType, DataType>
{
    protected Dictionary<EventType, HashSet<Action<DataType>>> _events = new Dictionary<EventType, HashSet<Action<DataType>>>();

    /// <summary>
    /// Clears all registered events.
    /// </summary>
    public virtual void Clear()
    {
        _events.Clear();
    }

    /// <summary>
    /// Clears all observers for a specific event type.
    /// </summary>
    public virtual void ClearType(EventType type)
    {
        _events.Remove(type);
    }

    /// <summary>
    /// Registers an observer for a specific event type.
    /// </summary>
    public virtual void RegisterEvent(EventType eventType, Action<DataType> observer)
    {
        if (!_events.ContainsKey(eventType))
        {
            _events[eventType] = new HashSet<Action<DataType>>();
        }
        _events[eventType].Add(observer);
    }

    /// <summary>
    /// Unregisters an observer from a specific event type.
    /// </summary>
    public virtual void UnRegisterEvent(EventType eventType, Action<DataType> observer)
    {
        if (_events.ContainsKey(eventType))
        {
            _events[eventType].Remove(observer);
            if (_events[eventType].Count == 0)
            {
                _events.Remove(eventType);
            }
        }
    }

    /// <summary>
    /// Notifies all observers of a specific event type with provided data.
    /// </summary>
    public virtual void Notify(EventType eventType, DataType data)
    {
        if (_events.ContainsKey(eventType))
        {
            foreach (var observer in _events[eventType])
            {
                observer?.Invoke(data);
            }
        }
    }
}
