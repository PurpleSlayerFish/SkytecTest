using PurpleSlayerFish.Core.Windows.Dialogs;

namespace PurpleSlayerFish.Core.Windows.Container
{
    public interface IUiContainer
    {
        T Get<T>() where T : AbstractController;
        void Show<T>() where T : AbstractController;
        void Hide<T>() where T : AbstractController;
        DialogBuilder BuildDialog();
    }
}