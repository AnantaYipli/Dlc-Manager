using Microsoft.SqlServer.Server;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public enum ObjectType { none, Scene, Scriptable }

public class AssetBundleManager : EditorWindow
{
    ObjectType objectType;

    Object testObj;
    bool isCustome = false;
    bool w = false, a = false, i = false, m = false;
    public string bundleName;

    const string dlcExt = "yiplidlc";

    [MenuItem("Utility/Assset Bundle Browser")]
    static void Init()
    {
        AssetBundleManager window = (AssetBundleManager)EditorWindow.GetWindowWithRect(typeof(AssetBundleManager), new Rect(0, 0, 400, 500));
        window.Show();
    }


    void OnGUI()
    {
        #region GUI Style
        GUIStyle _guiHeader = new GUIStyle()
        {
            fontSize = 26,
            richText = true,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            padding = new RectOffset(0, 0, 5, 0)
        };
        _guiHeader.normal.textColor = Color.white;

        GUIStyle _guiLable = new GUIStyle()
        {
            fontSize = 12,
            richText = true,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(10, 10, 3, 3)
        };
        _guiLable.normal.textColor = Color.white;

        GUILayoutOption[] _lableOpt = new GUILayoutOption[]
        {
            GUILayout.Width(150)
        };
        GUILayoutOption[] _fieldOpt = new GUILayoutOption[]
        {
            GUILayout.MinWidth(200)
        };
        #endregion

        GUILayout.Label("Asset Bundle Creator", _guiHeader, new GUILayoutOption[] { GUILayout.Height(40) });

        DrawUILine(Color.gray);

        EditorStyles.objectField.normal.textColor = Color.gray;
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("Asset Type", _guiLable, _lableOpt);
            objectType = (ObjectType)EditorGUILayout.EnumPopup(objectType, _fieldOpt);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        switch (objectType)
        {
            case ObjectType.Scene:
                GUILayout.Label("Scene", _guiLable, _lableOpt);
                testObj = EditorGUILayout.ObjectField(testObj, typeof(SceneAsset), false, _fieldOpt);
                break;
            case ObjectType.Scriptable:
                GUILayout.Label("Scriptable", _guiLable, _lableOpt);
                testObj = EditorGUILayout.ObjectField(testObj, typeof(ScriptableObject), false, _fieldOpt);
                break;
            default:
                testObj = null; break;
        }
        EditorGUILayout.EndHorizontal();

        if (testObj != null)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Custome Name", _guiLable, _lableOpt);
                isCustome = EditorGUILayout.Toggle(isCustome, _fieldOpt);
            }
            EditorGUILayout.EndHorizontal();

            if (isCustome)
            {
                if (string.IsNullOrEmpty(bundleName) || string.IsNullOrWhiteSpace(bundleName))
                    bundleName = testObj.name.ToLower();

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Bundle Name", _guiLable, _lableOpt);
                    bundleName = EditorGUILayout.TextField(bundleName, _fieldOpt);
                }
                EditorGUILayout.EndHorizontal();

                string _lable = string.Format("dlc/{0}/{1}.{2}", GetBundleName(objectType), bundleName.ToLower(), dlcExt);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUIStyle _guiLable2 = new GUIStyle()
                {
                    fontSize = 12,
                    richText = true,
                    fontStyle = FontStyle.Italic,
                    alignment = TextAnchor.MiddleLeft,
                    margin = new RectOffset(150, 0, 0, 0)
                };
                _guiLable2.normal.textColor = Color.gray;
                GUILayout.Label(_lable, _guiLable2, new GUILayoutOption[] { GUILayout.Width(300) });
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                string _lable = string.Format("dlc/{0}/{1}.{2}", GetBundleName(objectType), testObj.name.ToLower(), dlcExt);

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Bundle Name", _guiLable, _lableOpt);
                    EditorGUILayout.LabelField(_lable, EditorStyles.label, _fieldOpt);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            {
                GUILayout.Label("Platforms");

                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    w = EditorGUILayout.Toggle("Windows", w, new GUILayoutOption[] { GUILayout.Width(200) });
                    a = EditorGUILayout.Toggle("Android", a, new GUILayoutOption[] { GUILayout.Width(200) });

                    i = EditorGUILayout.Toggle("iOS", i, new GUILayoutOption[] { GUILayout.Width(200) });
                    m = EditorGUILayout.Toggle("macOS", m, new GUILayoutOption[] { GUILayout.Width(200) });
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        //ShowNotification(new GUIContent("No object selected for searching"));
        //DrawUILine(Color.gray);


        /* if (GUILayout.Button("Set name"))
         { CreateAssetBundleTag(); }

         if (GUILayout.Button("Create"))
         { CreateBundle(); }

         if (GUILayout.Button("Load"))
         { LoadBundle(); }*/
    }



    static void ReadOnlyTextField(string label, string text)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtility.labelWidth - 4));
            EditorGUILayout.SelectableLabel(text, EditorStyles.label, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        }
        EditorGUILayout.EndHorizontal();
    }

    static string GetBundleName(ObjectType _type)
    {
        string typeString = string.Empty;
        switch (_type)
        {
            case ObjectType.Scene: typeString = "Scene"; break;
            case ObjectType.Scriptable: typeString = "Scriptable"; break;
        }
        return typeString;
    }

    static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
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