using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using Object = UnityEngine.Object;

public class AssetBundleManager : EditorWindow
{
    public Object testObj;

    public string bundleName;

    [MenuItem("Window/My Window")]
    static void Init()
    {
        AssetBundleManager window = (AssetBundleManager)EditorWindow.GetWindow(typeof(AssetBundleManager));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        bundleName = EditorGUILayout.TextField("Text Field", bundleName);
        testObj = EditorGUILayout.ObjectField(testObj, typeof(Object), true);


        if (GUILayout.Button("Set name"))
        { CreateAssetBundleTag(); }

        if (GUILayout.Button("Create"))
        { CreateBundle(); }

        if (GUILayout.Button("Load"))
        { LoadBundle(); }
    }

    private void CreateAssetBundleTag()
    {
        AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(testObj)).SetAssetBundleNameAndVariant("dlc/" + bundleName + ".unity3d", "");
    }

    [ContextMenu("Create Bundle")]
    private void CreateBundle()
    {
        string windowsBundle = this.CreateWindowsBundle();
        //string iosBundle = this.CreateIosBundle();
        //string androidBundle = this.CreateAndroidBundle();
        //string macBundle = this.CreateMacBundle();
        //RemoveAssetBundleTags(this.assetCloneTempPath);
        //File.Delete(this.assetCloneTempPath);
        //File.Delete(this.assetCloneTempPath + ".meta");
        //Directory.Delete(this.window.assetBundleRootFolderPath, true);
        //File.Delete(this.window.assetBundleRootFolderPath + ".meta");
        Debug.Log(windowsBundle);
        AssetDatabase.Refresh();
    }

    private string CreateWindowsBundle()
    {
        string windowsBundle = Path.Combine(Application.dataPath, testObj.name, "Windows", testObj.name + ".unity3d");
        string path = Path.Combine(Application.dataPath, testObj.name, "Windows");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression;//   (BuildAssetBundleOptions)((BuildAssetBundleOptions)((BuildAssetBundleOptions)0 | 256) | 32);
        BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
        BuildPipeline.BuildAssetBundles(path, assetBundleOptions, buildTarget);
        return windowsBundle;
    }

    void LoadBundle()
    {
        //AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, "test1.unity3d"));
        AssetBundle.UnloadAllAssetBundles(true);
        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "Test Obj/Windows/dlc", bundleName + ".unity3d"));

        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        GameObject[] assets = myLoadedAssetBundle.LoadAllAssets<GameObject>();

        //var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("MyObject");
        Instantiate(assets[0]);

        myLoadedAssetBundle.Unload(false);
    }


    /* private string CreateIosBundle()
    {
        string iosBundle = Path.Combine(this.window.assetBundleRootFolderPath, this.selectedAsset.name, "iOS", this.selectedAsset.name, this.cloneAssetName + ".unity3d");
        string path = Path.Combine(this.window.assetBundleRootFolderPath, this.selectedAsset.name, "iOS");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        BuildAssetBundleOptions assetBundleOptions = (BuildAssetBundleOptions)((BuildAssetBundleOptions)((BuildAssetBundleOptions)0 | 256) | 32);
        BuildTarget buildTarget = (BuildTarget)9;
        BuildPipeline.BuildAssetBundles(path, assetBundleOptions, buildTarget);
        return iosBundle;
    }

    private string CreateAndroidBundle()
    {
        string androidBundle = Path.Combine(this.window.assetBundleRootFolderPath, this.selectedAsset.name, "Android", this.selectedAsset.name, this.cloneAssetName + ".unity3d");
        string path = Path.Combine(this.window.assetBundleRootFolderPath, this.selectedAsset.name, "Android");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        BuildAssetBundleOptions assetBundleOptions = (BuildAssetBundleOptions)((BuildAssetBundleOptions)((BuildAssetBundleOptions)0 | 256) | 32);
        BuildTarget buildTarget = (BuildTarget)13;
        BuildPipeline.BuildAssetBundles(path, assetBundleOptions, buildTarget);
        return androidBundle;
    }

    private string CreateWindowsBundle()
    {
        string windowsBundle = Path.Combine(this.window.assetBundleRootFolderPath, this.selectedAsset.name, "Windows", this.selectedAsset.name, this.cloneAssetName + ".unity3d");
        string path = Path.Combine(this.window.assetBundleRootFolderPath, this.selectedAsset.name, "Windows");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        BuildAssetBundleOptions assetBundleOptions = (BuildAssetBundleOptions)((BuildAssetBundleOptions)((BuildAssetBundleOptions)0 | 256) | 32);
        BuildTarget buildTarget = (BuildTarget)19;
        BuildPipeline.BuildAssetBundles(path, assetBundleOptions, buildTarget);
        return windowsBundle;
    }

    private string CreateMacBundle()
    {
        string macBundle = Path.Combine(this.window.assetBundleRootFolderPath, this.selectedAsset.name, "Mac", this.selectedAsset.name, this.cloneAssetName + ".unity3d");
        string path = Path.Combine(this.window.assetBundleRootFolderPath, this.selectedAsset.name, "Mac");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        BuildAssetBundleOptions assetBundleOptions = (BuildAssetBundleOptions)((BuildAssetBundleOptions)((BuildAssetBundleOptions)0 | 256) | 32);
        BuildTarget buildTarget = (BuildTarget)2;
        BuildPipeline.BuildAssetBundles(path, assetBundleOptions, buildTarget);
        return macBundle;
    }

    private static void RemoveAssetBundleTags(string path)
    {
        string implicitAssetBundleName = AssetDatabase.GetImplicitAssetBundleName(path);
        string[] bundleDependencies = AssetDatabase.GetAssetBundleDependencies(implicitAssetBundleName, true);
        AssetDatabase.RemoveAssetBundleName(implicitAssetBundleName, true);
        foreach (string str in bundleDependencies)
            AssetDatabase.RemoveAssetBundleName(str, true);
    }*/
}