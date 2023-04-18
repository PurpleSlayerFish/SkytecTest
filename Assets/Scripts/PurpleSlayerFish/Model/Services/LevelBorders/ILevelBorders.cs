using UnityEngine;

namespace PurpleSlayerFish.Model.Services.LevelBorders
{
    public interface ILevelBorders
    {
        Vector2 Border0 { get; }
        Vector2 Border1 { get; }
        Vector2 OuterBorder0 { get; }
        Vector2 OuterBorder1 { get; }
        Vector2 InnerBorder0 { get; }
        Vector2 InnerBorder1 { get; }
        Vector2 DeathzoneBorder0 { get; }
        Vector2 DeathzoneBorder1 { get; }

        void InitAllBorders();

        bool RemapBorders(in Vector2 target, out Vector2 newPosition);
    }
}