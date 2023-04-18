using PurpleSlayerFish.Core.Model;

namespace PurpleSlayerFish.Model.Services.Spawners
{
    public interface ISpawner<T> 
        where T : class, IEntity
    {
        // todo переделать после рефакторинга PoolAdapter, сделать дефолтную реализацию.
        // реализовать абстрактный класс с полями
        // T LinkViewModel<T0>(T0 view) where T0 : AbstractView;
        
        T Spawn();
        void Release(T entity);
    }
}