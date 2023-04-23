using System;
using PurpleSlayerFish.Core.Ui.ElementManager;
using Zenject;

namespace PurpleSlayerFish.Core.Ui.Dialogs
{
    public class DialogBuilder
    {
        private DialogWindow _dialog;
        private DialogController _controller;
        private IUiElementManager _uiElementManager;

        public DialogBuilder(DialogWindow dialog, IUiElementManager uiUIElementManager, DiContainer container)
        {
            _dialog = dialog;
            _uiElementManager = uiUIElementManager;
            _controller = _dialog.Initialize<DialogController>(container);
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
        
        public DialogBuilder WithButton(string text, Action action, bool finishDialog = false)
        {
            if (finishDialog)
                action += () => _controller.Hide();
            _controller.AddButton(_uiElementManager.BuildButton().WithText(text).WithAction(action).Build());
            return this;
        }

        public DialogController Build() => _controller;
    }
}