using System;

namespace PurpleSlayerFish.Core.Presenter
{
    public interface IPresenter : IDisposable
    {
        void SubscribeToEvents();
        void UnsubscribeToEvents();
    }
}