using PurpleSlayerFish.Core.Ui.ElementManager.Elements;
using PurpleSlayerFish.Core.Ui.Windows;

namespace PurpleSlayerFish.Core.Ui.Dialogs
{
    public class DialogController : AbstractController<DialogWindow>
    {
        public void SetLabel(string text) => _window.Label.text = text;
        public void SetDescription(string text) => _window.Description.text = text;
        public void AddButton(ExtendedButton button) => button.transform.SetParent(_window.ButtonLayout.transform);
        
        // todo dialog pool and release
        public override void Hide() => Dispose();
    }
}