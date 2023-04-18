using System;
using PurpleSlayerFish.Core.Windows.ElementManager;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;

namespace PurpleSlayerFish.Core.Windows.Dialogs
{
    public class DialogBuilder
    {
        private DialogWindow _dialog;
        private ISubscriptionObserver _subscriptionObserver;
        private DialogController _controller;
        private IUiElementManager _uiElementManager;

        public DialogBuilder(DialogWindow dialog, IUiElementManager uiUIElementManager, ISubscriptionObserver subscriptionObserver)
        {
            _dialog = dialog;
            _subscriptionObserver = subscriptionObserver;
            _uiElementManager = uiUIElementManager;
            _controller = _dialog.Initialize<DialogController>(_subscriptionObserver);
        }

        public DialogBuilder WithLabel(string text)
        {
            _controller.SetLabel(text);
            return this;
        }
        
        public DialogBuilder WithDescription(string text)
        {
            _controller.SetDescription(text);
            return this;
        }
        
        public DialogBuilder WithButton(string text, Action action)
        {
            _controller.AddButton(_uiElementManager.BuildButton().WithText(text).WithAction(action).Build());
            return this;
        }

        public DialogController Build() => _controller;
    }
}