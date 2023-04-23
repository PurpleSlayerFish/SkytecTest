using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PurpleSlayerFish.Core.Ui.ElementManager.Elements
{
    public class ExtendedButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _label;
        public Button Button => _button;
        public TMP_Text Label => _label;

        public void AddOnClick([NotNull] Action action) => Button.onClick.AddListener(new UnityAction(action));
    }
    
    public class ButtonBuilder
    {
        private ExtendedButton _extendedButton;

        public ButtonBuilder(ExtendedButton extendedButton)
        {
            _extendedButton = extendedButton;
        }

        [NotNull]
        public ButtonBuilder WithText(string text)
        {
            _extendedButton.Label.text = text;
            return this;
        }
        
        [NotNull]
        public ButtonBuilder WithAction(Action action)
        {
            _extendedButton.AddOnClick(action);
            return this;
        }

        public ExtendedButton Build() => _extendedButton;

    }
}