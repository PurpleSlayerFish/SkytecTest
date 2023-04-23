using TMPro;

namespace PurpleSlayerFish.Core.Ui.Windows.GameWindow
{
    public class GameWindow : AbstractWindow<GameController>
    {
        public TMP_Text Score;
        public TMP_Text MaxScore;
        public TMP_Text Position;
        public TMP_Text Rotation;
        public TMP_Text Velocity;
        public TMP_Text LaserCount;
        public TMP_Text LaserRestoration;
    }
}