using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundle
{
    [MenuItem("AssetBundle/Build AssetBundle")]
    static void BuildAssetBundle()
    {
        string streamPath = Application.streamingAssetsPath;
        if (Directory.Exists(streamPath))
        {
            Directory.Delete(streamPath,true);
        }
        Directory.CreateDirectory(streamPath);
        // 资源管理器刷新
        AssetDatabase.Refresh();
        //LZMA压缩结果体积较小，解压时间较长BuildAssetBundleOptions.None
        //LZ4压缩结果体积较大，解压时间较短 BuildAssetBundleOptions.ChunkBasedCompression
        BuildPipeline.BuildAssetBundles(streamPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
