using System;
using System.Collections.Generic;
using UnityEditor;

namespace TX_Randomizer
{
    public class MapNameDictionary : StringDictionary<Map.Name>
    {
#if UNITY_EDITOR
        [MenuItem("Create/StringDictionary/Map Name Dictionary")]

        static void CreateMapNameDictionary()
        {
            // MyClass is inheritant from ScriptableObject base class
            MapNameDictionary example = CreateInstance<MapNameDictionary>();
            // path has to start at "Assets"
            string path = "Assets/Scripts/ScriptableObjects/StringDictionaries/MapName_Dictionary.asset";

            example.Dictionary = new List<EnumToStringElement<Map.Name>>();
            Map.Name[] MapNameValues = (Map.Name[])Enum.GetValues(typeof(Map.Name));

            for (int i = 0; i < MapNameValues.Length; i++)
            {
                EnumToStringElement<Map.Name> element;
                element.KeyElement = MapNameValues[i];
                element.ValueElement = element.KeyElement.ToString();

                example.Dictionary.Add(element);
            }

            AssetDatabase.CreateAsset(example, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = example;
        }
#endif
    }
}