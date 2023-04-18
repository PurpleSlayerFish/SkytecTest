using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PurpleSlayerFish.Model.Services.PrefabProvider
{
    public class AssetBundlePrefabProvider : IPrefabProvider
    {
        public const string BUNDLES_FOLDER = "AssetBundles/Bundles";

        private Dictionary<string, AssetBundle> _assetBundles;
        private AssetBundle _assetBundle;

        public AssetBundlePrefabProvider()
        {
            _assetBundles = new Dictionary<string, AssetBundle>();
        }

        public T Get<T>(string bundleName, string prefabName) where T : Object
        {
            GetAssetBundle(bundleName, out _assetBundle);
            var result = _assetBundle.LoadAsset<T>(prefabName);
            if (result == null)
                throw new KeyNotFoundException("There is not prefab with given name '" + prefabName + "' or given type '" + typeof(T).Name + "' in bundle named '" + bundleName + "'.");
            return result;
        }

        public T GetComponent<T>(string bundleName, string prefabName) where T : Component => Get<GameObject>(bundleName, prefabName).GetComponent<T>();

        public Object[] Get(string bundleName)
        {
            GetAssetBundle(bundleName, out _assetBundle);
            var results = _assetBundle.LoadAllAssets();
            if (results == null)
                throw new KeyNotFoundException("There is not prefabs with name in bundle named '" + bundleName + "'.");
            return results;
        }

        private void GetAssetBundle(string bundleName, out AssetBundle assetBundle)
        {
            if (_assetBundles.ContainsKey(bundleName))
                assetBundle = _assetBundles[bundleName];
            else
            {
                assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, BUNDLES_FOLDER + "/" + bundleName));
                if (assetBundle == null)
                    throw new ArgumentException("Failed to load AssetBundle!");
                _assetBundles.Add(bundleName, assetBundle);
            }
        }

        public void Dispose()
        {
            var enumerator = _assetBundles.GetEnumerator();
            while (enumerator.MoveNext())
                enumerator.Current.Value.Unload(true);
        }
    }
}