using PurpleSlayerFish.Core.Windows.ElementManager.Elements;
using UnityEngine;

namespace PurpleSlayerFish.Core.Windows.Dialogs
{
    public class DialogController : AbstractController<DialogWindow>
    {
        public void SetLabel(string text) => _window.Label.text = text;
        public void SetDescription(string text) => _window.Description.text = text;
        public void AddButton(ExtendedButton button) => button.transform.SetParent(_window.ButtonLayout.transform);
        
        // todo dialog pool and release
        public override void Hide() => Object.Destroy(_window);
    }
}