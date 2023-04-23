using System;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Services.Pools.PoolProvider;
using PurpleSlayerFish.Core.View;
using Zenject;

namespace PurpleSlayerFish.Core.Services.Pools.PoolAdapter
{
    public class PoolAdapter<T1, T2> 
        where T1 : class, IPoolableEntity, new()
        where T2 : AbstractView<T2>
    {
        [Inject] private IAdaptablePoolProvider _poolProvider;
        private string _poolKey;
        private Func<T2,T1> _linkViewModelFunc;

        public string PoolKey
        {
            get => _poolKey;
            set => _poolKey = value;
        }

        public Func<T2, T1> LinkViewModelFunc
        {
            get => _linkViewModelFunc;
            set => _linkViewModelFunc = value;
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