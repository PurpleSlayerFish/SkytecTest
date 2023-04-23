using PurpleSlayerFish.Core.Ui.Dialogs;
using PurpleSlayerFish.Core.Ui.Windows;
using Zenject;

namespace PurpleSlayerFish.Core.Ui.Container
{
    public interface IUiContainer
    {
        void Install(DiContainer container);
        void InitializeWindow<T>(bool inRoot = true) where T : AbstractWindow;
        T Get<T>() where T : AbstractController;
        void Show<T>() where T : AbstractController;
        void Hide<T>() where T : AbstractController;
        void ClearRoot();
        DialogBuilder BuildDialog();
    }
}