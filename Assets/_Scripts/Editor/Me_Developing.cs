using UnityEditor;
using UnityEngine;

public class Me_Developing : EditorWindow
{

    [MenuItem("My Tools/Me Developing", false, 1)]
    public static void ShowWindow()
    {
        GetWindow(typeof(Me_Developing));
    }

    public void OnGUI()
    {
        // Layout the UI
    }
}