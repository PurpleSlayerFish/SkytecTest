using Newtonsoft.Json;
using UnityEngine;

namespace PurpleSlayerFish.Core.Services.DataStorage
{
    public class PlayerPrefsStorage<T> : IDataStorage<T>
        where T : struct
    {
        public const string PLAYER_PREFS_KEY = "data";
        
        public string _jsonData;
        // private PlayerData _cachedData;
        
        public void Save(T data) => PlayerPrefs.SetString(PLAYER_PREFS_KEY, JsonConvert.SerializeObject(data));

        public T Load()
        {
            _jsonData = PlayerPrefs.GetString(PLAYER_PREFS_KEY);
            if (_jsonData == "")
                return default;
            return JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(PLAYER_PREFS_KEY));
        }

        public void Clear() => PlayerPrefs.DeleteAll();
    }
    
}