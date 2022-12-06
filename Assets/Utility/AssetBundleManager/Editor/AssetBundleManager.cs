using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public enum ObjectType { none, Scene, Scriptable }

public class AssetBundleManager : EditorWindow
{
    ObjectType objectType;

    Object bundleObject;
    bool isCustome = false;
    bool w = false, a = false, i = false, m = false;
    string bNameField;

    string bundleName = string.Empty;
    const string dlcExt = "yiplidlc";

    static string basePath = string.Empty;

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
        GUIStyle _guiLable2 = new GUIStyle()
        {
            fontSize = 12,
            richText = true,
            fontStyle = FontStyle.Italic,
            alignment = TextAnchor.MiddleLeft,
            margin = new RectOffset(155, 0, 0, 0)
        };
        _guiLable2.normal.textColor = Color.gray;
        GUIStyle _guiLable3 = new GUIStyle()
        {
            fontSize = 11,
            richText = true,
            fontStyle = FontStyle.Normal,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(5, 10, 3, 3)
        };
        _guiLable3.normal.textColor = Color.white;

        GUILayoutOption[] _lableOpt = new GUILayoutOption[]
        {
            GUILayout.Width(150)
        };
        GUILayoutOption[] _fieldOpt = new GUILayoutOption[]
        {
            GUILayout.MinWidth(200)
        };
        GUILayoutOption[] _buttonGroupOpt = new GUILayoutOption[]
        {
            GUILayout.Width(150)
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
                bundleObject = EditorGUILayout.ObjectField(bundleObject, typeof(SceneAsset), false, _fieldOpt);
                break;
            case ObjectType.Scriptable:
                GUILayout.Label("Scriptable", _guiLable, _lableOpt);
                bundleObject = EditorGUILayout.ObjectField(bundleObject, typeof(ScriptableObject), false, _fieldOpt);
                break;
            default:
                bundleObject = null; break;
        }
        EditorGUILayout.EndHorizontal();

        if (bundleObject != null)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Custome Name", _guiLable, _lableOpt);
                isCustome = EditorGUILayout.Toggle(isCustome, _fieldOpt);
            }
            EditorGUILayout.EndHorizontal();

            if (isCustome)
            {
                if (string.IsNullOrEmpty(bNameField) || string.IsNullOrWhiteSpace(bNameField))
                    bNameField = bundleObject.name.ToLower();

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Bundle Name", _guiLable, _lableOpt);
                    bNameField = EditorGUILayout.TextField(bNameField, _fieldOpt);
                }
                EditorGUILayout.EndHorizontal();

                bundleName = string.Format("dlc/{0}/{1}.{2}", GetBundleName(objectType), bNameField.ToLower(), dlcExt).ToLower();

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label(bundleName, _guiLable2, new GUILayoutOption[] { GUILayout.Width(300) });
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                bundleName = string.Format("dlc/{0}/{1}.{2}", GetBundleName(objectType), bundleObject.name.ToLower(), dlcExt).ToLower();

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Bundle Name", _guiLable, _lableOpt);
                    EditorGUILayout.LabelField(bundleName, EditorStyles.label, _fieldOpt);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Platforms", _guiLable, _lableOpt);

                EditorGUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(100) });
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (!w)
                            GUI.Label(new Rect(150, isCustome ? 160 : 145, 200, 10), "---------");
                        GUILayout.Label("Windows", _guiLable3, _lableOpt);
                        w = EditorGUILayout.Toggle(w, _fieldOpt);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (!a)
                            GUI.Label(new Rect(150, isCustome ? 180 : 165, 200, 10), "--------");
                        GUILayout.Label("Android", _guiLable3, _lableOpt);
                        a = EditorGUILayout.Toggle(a, _fieldOpt);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (!i)
                            GUI.Label(new Rect(150, isCustome ? 200 : 185, 200, 10), "----");
                        GUILayout.Label("iOS", _guiLable3, _lableOpt);
                        i = EditorGUILayout.Toggle(i, _fieldOpt);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (!m)
                            GUI.Label(new Rect(150, isCustome ? 220 : 205, 200, 10), "-------");
                        GUILayout.Label("macOS", _guiLable3, _lableOpt);
                        m = EditorGUILayout.Toggle(m, _fieldOpt);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        //ShowNotification(new GUIContent("No object selected for searching"));
        DrawUILine(Color.gray);

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUILayout.Space();
        {
            if (GUILayout.Button("Create Bundle", _buttonGroupOpt))
            {
                if (VerifyInfo())
                {
                    StartBuilding();
                }
            }

            /*if (GUILayout.Button("Create"))
            { CreateBundle(); }

            if (GUILayout.Button("Load"))
            { LoadBundle(); }*/
        }
        EditorGUILayout.EndHorizontal();
    }

    bool VerifyInfo()
    {
        if (string.IsNullOrEmpty(bundleName) || string.IsNullOrWhiteSpace(bundleName))
        {
            ShowNotification(new GUIContent("Bundle name cannot be empty !!!"), 1f);
            return false;
        }
        if (!w && !a && !i && !m)
        {
            ShowNotification(new GUIContent("No platoform selected !!!"), 1f);
            return false;
        }
        return true;
    }

    void StartBuilding()
    {
        foreach (string s in AssetDatabase.GetAllAssetBundleNames())
            AssetDatabase.RemoveAssetBundleName(s, true);

        CreateAssetBundleTag();
    }

    private void CreateAssetBundleTag()
    {
        AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(bundleObject)).SetAssetBundleNameAndVariant(bundleName, "");
        CheckDirector();
    }

    void CheckDirector()
    {
        basePath = Path.Combine(Application.dataPath, bundleObject.name);
        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        AssetDatabase.Refresh();

        CreateBundle();
    }

    private void CreateBundle()
    {
        if (w) CreateWindowsBundle();
        if (i) CreateIosBundle();
        if (a) CreateAndroidBundle();
        if (m) CreateMacOsBundle();

        AssetDatabase.RemoveAssetBundleName(bundleName, true);
        //RemoveAssetBundleTags(this.assetCloneTempPath);
        //File.Delete(this.assetCloneTempPath);
        //File.Delete(this.assetCloneTempPath + ".meta");
        //Directory.Delete(this.window.assetBundleRootFolderPath, true);
        //File.Delete(this.window.assetBundleRootFolderPath + ".meta");
        //Debug.Log(windowsBundle);
        AssetDatabase.Refresh();
    }

    private void CreateWindowsBundle()
    {
        try
        {
            string _path = Path.Combine(basePath, "windows");
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression;
            BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
            BuildPipeline.BuildAssetBundles(_path, assetBundleOptions, buildTarget);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    private void CreateIosBundle()
    {
        try
        {
            string _path = Path.Combine(basePath, "iOS");
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression;
            BuildTarget buildTarget = BuildTarget.iOS;
            BuildPipeline.BuildAssetBundles(_path, assetBundleOptions, buildTarget);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    private void CreateAndroidBundle()
    {
        try
        {
            string _path = Path.Combine(basePath, "android");
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression;
            BuildTarget buildTarget = BuildTarget.Android;
            BuildPipeline.BuildAssetBundles(_path, assetBundleOptions, buildTarget);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    private void CreateMacOsBundle()
    {
        try
        {
            string _path = Path.Combine(basePath, "macOs");
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression;
            BuildTarget buildTarget = BuildTarget.StandaloneOSX;
            BuildPipeline.BuildAssetBundles(_path, assetBundleOptions, buildTarget);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    /*private static void RemoveAssetBundleTags(string path)
    {
        string implicitAssetBundleName = AssetDatabase.GetImplicitAssetBundleName(path);
        string[] bundleDependencies = AssetDatabase.GetAssetBundleDependencies(implicitAssetBundleName, true);
        AssetDatabase.RemoveAssetBundleName(implicitAssetBundleName, true);
        foreach (string str in bundleDependencies)
            AssetDatabase.RemoveAssetBundleName(str, true);
    }*/

    void LoadBundle()
    {
        //AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, "test1.unity3d"));
        AssetBundle.UnloadAllAssetBundles(true);
        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "Test Obj/Windows/dlc", bNameField + ".unity3d"));

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


    #region Helpers
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
    #endregion
    /* 

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