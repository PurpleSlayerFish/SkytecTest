using PurpleSlayerFish.Core.Windows;
using PurpleSlayerFish.Windows.Controller;
using TMPro;

namespace PurpleSlayerFish.Windows.Window
{
    public class GameWindow : AbstractWindow<GameController>
    {
        public TMP_Text Score;
        public TMP_Text Position;
        public TMP_Text Rotation;
        public TMP_Text Velocity;
        public TMP_Text LaserCount;
        public TMP_Text LaserRestoration;
    }
}