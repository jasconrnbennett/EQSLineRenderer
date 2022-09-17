using UnityEditor;
using UnityEngine;

public class MenuItems
{
    [MenuItem("Tools/Clear PlayerPrefs %#d")]
    private static void NewMenuOption()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs have been deleted");
    }
}
