using System;
using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using UnityEngine;

namespace PurpleSlayerFish.Core.Services.SubscriptionObserver
{
    public class SubscriptionObserver : ISubscriptionObserver
    {
        // todo плохо написан, заменить на Zenject SignalBus, тогда не придется рефакторить
        private Dictionary<string, ISubscription> _subscriptions;
        
        public SubscriptionObserver()
        {
            _subscriptions = new Dictionary<string, ISubscription>();
        }

        public void Subscribe(string key, ISubscription subscription) => _subscriptions.Add(key, subscription);
        public void Subscribe(string key, Action onExecute) => _subscriptions.Add(key, new SimpleSubscription(onExecute));
        public void Subscribe<T>(string key, Action<T> onExecute) where T : class, IEntity => _subscriptions.Add(key, new EntitySubscription<T>(onExecute));
        public void Subscribe(string key, Action<Vector2> onExecute) => _subscriptions.Add(key, new Vector2Subscription(onExecute));
        public void Subscribe(string key, Action<float> onExecute) => _subscriptions.Add(key, new FloatSubscription(onExecute));
        public void Subscribe(string key, Action<int> onExecute) => _subscriptions.Add(key, new IntSubscription(onExecute));
        public void Unsubscribe(string key) => _subscriptions.Remove(key);
        public void Execute(string key) => _subscriptions[key].Execute();
        public void Execute<T>(string key, T entity) where T : class, IEntity => ((EntitySubscription<T>)_subscriptions[key]).Execute(entity);
        public void Execute(string key, Vector2 value) => ((Vector2Subscription)_subscriptions[key]).Execute(value);
        public void Execute(string key, float value) => ((FloatSubscription)_subscriptions[key]).Execute(value);
        public void Execute(string key, int value) => ((IntSubscription)_subscriptions[key]).Execute(value);

        public void Clear() => _subscriptions.Clear();
    }
    
    public class SimpleSubscription : ISubscription
    {
        private readonly Action _onExecute;

        public SimpleSubscription(Action onExecute)
        {
            _onExecute += onExecute;
        }
        
        public void Execute() => _onExecute.Invoke();
    }

    public class EntitySubscription<T> : ISubscription where T : class, IEntity
    {
        private T _entity;
        private readonly Action<T> _onExecute;

        public EntitySubscription(Action<T> onExecute)
        {
            _onExecute += onExecute;
        }

        public void Execute(T entity)
        {
            _entity = entity; 
            _onExecute.Invoke(_entity);
        }

        public void Execute() => _onExecute.Invoke(null);
    }

    public class Vector2Subscription : ISubscription
    {
        private Vector2 _vector;
        private readonly Action<Vector2> _onExecute;

        public Vector2Subscription(Action<Vector2> onExecute)
        {
            _onExecute += onExecute;
        }

        public void Execute(Vector2 value)
        {
            _vector = value;
            _onExecute.Invoke(_vector);
        }

        public void Execute() => _onExecute.Invoke(default);
    }
    
    public class FloatSubscription : ISubscription
    {
        private float _float;
        private readonly Action<float> _onExecute;

        public FloatSubscription(Action<float> onExecute)
        {
            _onExecute += onExecute;
        }

        public void Execute(float value)
        {
            _float = value;
            _onExecute.Invoke(_float);
        }

        public void Execute() => _onExecute.Invoke(default);
    }
    
    public class IntSubscription : ISubscription
    {
        private int _int;
        private readonly Action<int> _onExecute;

        public IntSubscription(Action<int> onExecute)
        {
            _onExecute += onExecute;
        }

        public void Execute(int value)
        {
            _int = value;
            _onExecute.Invoke(_int);
        }

        public void Execute() => _onExecute.Invoke(default);
    }
}