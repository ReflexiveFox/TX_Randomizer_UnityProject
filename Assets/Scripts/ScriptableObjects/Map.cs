using System;
using UnityEngine;
using UnityEditor;
namespace TX_Randomizer
{
#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "New Map", menuName = "Map")]
#endif
    public partial class Map : ScriptableObject
    {
        public Sprite Sprite;
        public Name MapName;

#if UNITY_EDITOR
        [MenuItem("Create/Maps")]
        static void CreateMaps()
        {
            Map.Name[] mapNameValues = (Map.Name[])Enum.GetValues(typeof(Map.Name));
            foreach(Map.Name mapName in mapNameValues)
            {
                // MyClass is inheritant from ScriptableObject base class
                Map instance = CreateInstance<Map>();
                // path has to start at "Assets"
                string path = $"Assets/Scripts/ScriptableObjects/Maps/{mapName}_Map.asset";

                instance.MapName = mapName;

                AssetDatabase.CreateAsset(instance, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = instance;
            }
        }
#endif
    }
}