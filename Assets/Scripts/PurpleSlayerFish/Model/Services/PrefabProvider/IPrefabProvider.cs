using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PurpleSlayerFish.Model.Services.PrefabProvider
{
    public interface IPrefabProvider : IDisposable
    {
        T Get<T>(string bundleName, string prefabName) where T : Object;
        T GetComponent<T>(string bundleName, string prefabName) where T : Component ;
        Object[] Get(string bundleName);
    }
}