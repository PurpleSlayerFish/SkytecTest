using System;
using JetBrains.Annotations;
using PurpleSlayerFish.Core.Windows.ElementManager.Elements;
using UnityEngine.Events;

namespace PurpleSlayerFish.Core.Windows.ElementManager
{
    public interface IUiElementManager
    {
        ExtendedButtonBuilder BuildButton();
    }

    public class ExtendedButtonBuilder
    {
        private ExtendedButton _extendedButton;

        public ExtendedButtonBuilder(ExtendedButton extendedButton)
        {
            _extendedButton = extendedButton;
        }

        [NotNull]
        public ExtendedButtonBuilder WithText(string text)
        {
            _extendedButton.Label.text = text;
            return this;
        }
        
        [NotNull]
        public ExtendedButtonBuilder WithAction(Action action)
        {
            _extendedButton.Button.onClick.AddListener(new UnityAction(action));
            return this;
        }

        public ExtendedButton Build() => _extendedButton;

    }
}