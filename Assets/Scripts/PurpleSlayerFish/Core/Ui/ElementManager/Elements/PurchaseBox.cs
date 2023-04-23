using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace PurpleSlayerFish.Core.Ui.ElementManager.Elements
{
    public class PurchaseBox : MonoBehaviour
    {
        [SerializeField] private ExtendedButton _button;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private TMP_Text _price;
        public ExtendedButton Button => _button;
        public TMP_Text Label => _label;
        public TMP_Text Price => _price;

        public void AddOnClick([NotNull] Action action) => Button.AddOnClick(action);
    }
    
    public class PurchaseBoxBuilder
    {
        private PurchaseBox _element;

        public PurchaseBoxBuilder(PurchaseBox element)
        {
            _element = element;
        }

        [NotNull]
        public PurchaseBoxBuilder WithLabel(string text)
        {
            _element.Label.text = text;
            return this;
        }
        
        [NotNull]
        public PurchaseBoxBuilder WithPrice(string text)
        {
            _element.Price.text = text;
            return this;
        }
        
        [NotNull]
        public PurchaseBoxBuilder WithAction(Action action)
        {
            _element.AddOnClick(action);
            return this;
        }

        public PurchaseBox Build() => _element;
    }
}