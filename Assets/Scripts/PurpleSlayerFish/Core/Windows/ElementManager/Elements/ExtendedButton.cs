using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PurpleSlayerFish.Core.Windows.ElementManager.Elements
{
    public class ExtendedButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text label;
        public Button Button => _button;
        public TMP_Text Label => label;
    }
}