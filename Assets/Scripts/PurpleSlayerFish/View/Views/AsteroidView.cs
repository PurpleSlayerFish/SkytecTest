using PurpleSlayerFish.Core.View;
using UnityEngine;

namespace PurpleSlayerFish.View.Views
{
    public class AsteroidView : AbstractView<AsteroidView>
    {
        [SerializeField] private SpriteRenderer _renderer;
        public SpriteRenderer Renderer => _renderer;
    }
}