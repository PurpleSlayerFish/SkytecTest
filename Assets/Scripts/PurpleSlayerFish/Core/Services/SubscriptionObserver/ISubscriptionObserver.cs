using System;
using PurpleSlayerFish.Core.Model;
using UnityEngine;

namespace PurpleSlayerFish.Core.Services.SubscriptionObserver
{
    public interface ISubscriptionObserver
    {
        void Subscribe(string key, ISubscription subscription);
        void Subscribe(string key, Action onExecute);
        void Subscribe<T>(string key, Action<T> onExecute) where T : class, IEntity;
        void Subscribe(string key, Action<Vector2> onExecute);
        void Subscribe(string key, Action<float> onExecute);
        void Subscribe(string key, Action<int> onExecute);
        void Unsubscribe(string key);
        void Execute(string key);
        void Execute<T>(string key, T entity) where T : class, IEntity;
        void Execute(string key, Vector2 value);
        void Execute(string key, float value);
        void Execute(string key, int value);
        void Clear();
    }

    public interface ISubscription
    {
        void Execute();
    }
}