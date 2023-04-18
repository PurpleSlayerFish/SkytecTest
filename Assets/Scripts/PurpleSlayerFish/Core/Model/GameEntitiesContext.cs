using System.Collections.Generic;

namespace PurpleSlayerFish.Core.Model
{
    public class GameEntitiesContext : IEntitiesContext
    {
        private Dictionary<string, List<IEntity>> _contextDictionary;
        private List<IEntity> _tempList;

        public GameEntitiesContext()
        {
            _contextDictionary = new Dictionary<string, List<IEntity>>();
        }

        public void Insert<T>(string typeKey, T entity) where T : IEntity
        {
            if (!_contextDictionary.ContainsKey(typeKey))
                _contextDictionary.Add(typeKey, new List<IEntity>());
            _contextDictionary[typeKey].Add(entity);
        }
        
        public List<IEntity> Select(string typeKey)
        {
            if (!_contextDictionary.TryGetValue(typeKey, out _tempList))
                return null;
            return _tempList;
        }

        public T SelectFirst<T>(string typeKey) where T : class, IEntity
        {
            _tempList = Select(typeKey);
            return _tempList == null || _tempList.Count == 0 ? null : _tempList[0] as T;
        }

        public void Remove<T>(string typeKey, T entity) where T : class, IEntity
        {
            if (!_contextDictionary.TryGetValue(typeKey, out _tempList))
                throw new KeyNotFoundException();
            _tempList.Remove(entity);
        }
    }
}