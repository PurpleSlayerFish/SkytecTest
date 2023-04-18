using System.Collections.Generic;

namespace PurpleSlayerFish.Core.Model
{
    public interface IEntitiesContext
    {
        // TODO Можно было сделать через nameof,
        // но в лучшем стеке на месте этого сервиса будет ECS,
        // так что переделывать смысла не имеет
        void Insert<T>(string typeKey, T entity) where T : IEntity;
        List<IEntity> Select(string typeKey);
        T SelectFirst<T>(string typeKey) where T : class, IEntity;
        void Remove<T>(string typeKey, T entity) where T : class, IEntity;
    }
}