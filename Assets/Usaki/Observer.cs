using System;
using System.Collections.Generic;
using Utils.Singletons;

public class Observer : ZMonoSingleton<Observer>
{
    private Dictionary<string, HashSet<Action<object>>> _observerTopics = new Dictionary<string, HashSet<Action<object>>>();

    public void AddObserver(string topicName, Action<object> observer)
    {
        CreateObserverTopic(topicName);
        _observerTopics[topicName].Add(observer);
    }

    public void RemoveObserver(string topicName, Action<object> observer)
    {
        if (_observerTopics.ContainsKey(topicName))
        {
            _observerTopics[topicName].Remove(observer);
            if (_observerTopics[topicName].Count == 0)
            {
                _observerTopics.Remove(topicName);
            }
        }
    }

    public void RemoveObserverTopic(string topicName)
    {
        _observerTopics.Remove(topicName);
    }

    public void Notify(string topicName)
    {
        NotifyWithData(topicName, null);
    }

    public void NotifyWithData(string topicName, object data)
    {
        if (_observerTopics.ContainsKey(topicName))
        {
            foreach (var observer in _observerTopics[topicName])
            {
                observer?.Invoke(data);
            }
        }
    }

    protected HashSet<Action<object>> CreateObserverTopic(string topicName)
    {
        if (!_observerTopics.ContainsKey(topicName))
        {
            _observerTopics[topicName] = new HashSet<Action<object>>();
        }
        return _observerTopics[topicName];
    }
}
