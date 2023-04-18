using System.IO;
using UnityEditor;

namespace Editor
{
    public class AssetBundles
    {
        public const string BUNDLES_FOLDER = "Assets/AssetBundles/Bundles";
        
        [MenuItem("Assets/Create Assets Bundles")]
        private static void BuildAllAssetBundles()
        {
            var path = BUNDLES_FOLDER;
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
}