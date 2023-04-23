using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PurpleSlayerFish.Core.Services.PrefabProvider
{
    public interface IPrefabProvider : IDisposable
    {
        T Get<T>(string bundleName, string prefabName) where T : Object;
        T GetComponent<T>(string bundleName, string prefabName) where T : Component ;
        Object[] Get(string bundleName);

        T Instantiate<T>(string bundleName, string prefabName) where T : Component;

        T Instantiate<T>(string bundleName, string prefabName, Transform parent) where T : Component;

        T Instantiate<T>(GameObject prefab) where T : Component;

        T Instantiate<T>(GameObject prefab, Transform parent) where T : Component;
    }
}