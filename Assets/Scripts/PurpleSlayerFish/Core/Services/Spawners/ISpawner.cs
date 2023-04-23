using PurpleSlayerFish.Core.Model;
using Zenject;

namespace PurpleSlayerFish.Core.Services.Spawners
{
    public interface ISpawner<T> 
        where T : class, IEntity
    {
        // todo переделать после рефакторинга PoolAdapter, сделать дефолтную реализацию.
        // реализовать абстрактный класс с полями
        // T LinkViewModel<T0>(T0 view) where T0 : AbstractView;

        void Init(DiContainer container);
        T Spawn();
        void Release(T entity);
        void CheckDeathzone();
    }
}