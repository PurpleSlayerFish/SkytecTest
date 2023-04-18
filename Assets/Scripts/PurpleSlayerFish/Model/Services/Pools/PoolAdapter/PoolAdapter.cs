using System;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.View;
using PurpleSlayerFish.Model.Services.Pools.PoolProvider;

namespace PurpleSlayerFish.Model.Services.Pools.PoolAdapter
{
    public class PoolAdapter<T1, T2> 
        where T1 : class, IPoolableEntity, new()
        where T2 : AbstractView<T2>
    {
        private IPoolProvider _poolProvider;
        private string _poolKey;
        private Func<T2,T1> _linkViewModelFunc;

        public PoolAdapter(IPoolProvider poolProvider, string poolKey, Func<T2, T1> linkViewModelFunc)
        {
            _poolProvider = poolProvider;
            _poolKey = poolKey;
            _linkViewModelFunc = linkViewModelFunc;
        }

        public T1 Get() => _linkViewModelFunc.Invoke(_poolProvider.Get(_poolKey) as T2);

        public void Release(IPoolableEntity entity)
        {
            _poolProvider.Release(_poolKey, entity.View);
            entity.View = null;
        }
    }
    
    public interface IPoolableEntity : IEntity
    {
        // TODO переделать адаптеры на динамическую типизацию,
        // избавиться от конструкторов в презентерах, оставив стандарный,
        // написать билдер/фабрику, который будет принимать генерик и возвращать созданный презентер,
        // убрать ссылку на вью
        AbstractView View { get; set; }
    }
}