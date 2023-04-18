using System;
using System.Threading.Tasks;

namespace PurpleSlayerFish.Core.View
{
    public class Temporator
    {
        private int _duration;
        private Action _onComplete;

        public Temporator(int duration, Action onComplete)
        {
            _duration = duration;
            _onComplete = onComplete;
        }

        public async void Execute()
        {
            await Task.Delay(_duration);
            _onComplete?.Invoke();
        }
    }
}