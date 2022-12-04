using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utility.DlcManager
{
    public class DlcBrowser : EditorWindow
    {
        [MenuItem("Utility/Dlc Browser")]
        static void Init()
        {
            DlcBrowser window = (DlcBrowser)EditorWindow.GetWindow(typeof(DlcBrowser));
            window.Show();
        }
    }
}