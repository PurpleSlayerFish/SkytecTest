using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PurpleSlayerFish.Core.Windows.Dialogs
{
    public class DialogWindow : AbstractWindow<DialogController>
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private LayoutGroup _buttonLayout;
        public TMP_Text Label => _label;
        public TMP_Text Description => _description;
        public LayoutGroup ButtonLayout => _buttonLayout;
    }
}