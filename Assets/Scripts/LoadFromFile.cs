using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadFromFile : MonoBehaviour
{
    private int version;
    // Start is called before the first frame update
    void Start()
    {
        version = 9;
        LoadLua("lua.unity3d", new string[] { "Main.lua", "TestPlayer.lua" });
        LoadPrefab("testplayer.unity3d");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadPrefab(string objAssetBundleFile)
    {
        //string path = Application.streamingAssetsPath + "/testplayer.unity3d";
        //AssetBundle ab = AssetBundle.LoadFromMemory(File.ReadAllBytes(path));
        //AssetBundle ab = AssetBundle.LoadFromFile(path);

        LoadManifesFromOjb(objAssetBundleFile);
        string path = "file://" + Application.streamingAssetsPath + "/"+ objAssetBundleFile;
        WWW www = WWW.LoadFromCacheOrDownload(path, version);
        AssetBundle ab = www.assetBundle;
        UnityEngine.Object[] obj = ab.LoadAllAssets<GameObject>();
        foreach (var o in obj)
        {
            Instantiate(o); 
        }
    }

    void LoadManifesFromOjb(string objAssetBundleFile)
    {
        string path = Application.streamingAssetsPath + "/StreamingAssets";
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        AssetBundleManifest assetBundleManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] strs = assetBundleManifest.GetAllDependencies(objAssetBundleFile);

        foreach(var s in strs)
        {
            AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + s);
        }
    }

    void LoadLua(string luaAssetBundleFIle,string[] luaFileNames)
    {
        string path = "file://" + Application.streamingAssetsPath + "/" + luaAssetBundleFIle;
        WWW www = WWW.LoadFromCacheOrDownload(path, version);
        AssetBundle ab = www.assetBundle;
        foreach(var luaName in luaFileNames)
        {
            TextAsset text = ab.LoadAsset(luaName) as TextAsset;

            if (File.Exists(Application.dataPath + "/Lua/" + luaName))
            {
                File.Delete(Application.dataPath + "/Lua/" + luaName);
            }
            File.WriteAllBytes(Application.dataPath + "/Lua/" + luaName, text.bytes);
        }
    }
}
