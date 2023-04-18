using PurpleSlayerFish.Model.Services.ScriptableObjects.GameConfig;
using UnityEngine;

namespace PurpleSlayerFish.Model.Services.LevelBorders
{
    public class LevelBorders : ILevelBorders
    {
        private IGameConfig _gameConfig;
        private Camera _camera;
        private Vector2 _border0;
        private Vector2 _border1;
        private Vector2 _outerBorder0;
        private Vector2 _outerBorder1;
        private Vector2 _innerBorder0;
        private Vector2 _innerBorder1;
        private Vector2 _deathzoneBorder0;
        private Vector2 _deathzoneBorder1;

        public Vector2 Border0 => _border0;
        public Vector2 Border1 => _border1;
        public Vector2 OuterBorder0 => _outerBorder0;
        public Vector2 OuterBorder1 => _outerBorder1;
        public Vector2 InnerBorder0 => _innerBorder0;
        public Vector2 InnerBorder1 => _innerBorder1;
        public Vector2 DeathzoneBorder0 => _deathzoneBorder0;
        public Vector2 DeathzoneBorder1 => _deathzoneBorder1;

        public LevelBorders(Camera camera, IGameConfig gameConfig)
        {
            _camera = camera;
            _gameConfig = gameConfig;
        }

        public void InitAllBorders()
        {
            InitBorders();
            InitOuterBorders();
            InitInnerBorders();
            InitDeathzoneBorders();
        }

        public bool RemapBorders(in Vector2 target, out Vector2 newPosition)
        {
            newPosition = target;

            if (target.x < _border0.x)
                newPosition.x = _border1.x - (_border0.x - target.x);
            if (target.x > _border1.x)
                newPosition.x = _border0.x + (target.x - _border1.x);
            if (target.y < _border0.y)
                newPosition.y = _border1.y - (_border0.y - target.y);
            if (target.y > _border1.y)
                newPosition.y = _border0.y + (target.y - _border1.y);

            return newPosition != target;
        }

        private void InitBorders()
        {
            _border0 = _camera.ViewportToWorldPoint(Vector3.zero);
            _border1 = _camera.ViewportToWorldPoint(Vector3.one);
        }
        
        private void InitOuterBorders()
        {
            var offset = _gameConfig.OuterBorderOffset;
            _outerBorder0 = _border0 - Vector2.one * offset;
            _outerBorder1 = _border1 + Vector2.one * offset;
        }

        private void InitInnerBorders()
        {
            var offset = _gameConfig.InnerBorderOffset;
            _innerBorder0 = _border0 + Vector2.one * offset;
            _innerBorder1 = _border1 - Vector2.one * offset;
        }

        private void InitDeathzoneBorders()
        {
            var offset = _gameConfig.OuterBorderOffset;
            _deathzoneBorder0 = _border0 - Vector2.one * 2 * offset;
            _deathzoneBorder1 = _border1 + Vector2.one * 2 * offset;
        }
    }
}