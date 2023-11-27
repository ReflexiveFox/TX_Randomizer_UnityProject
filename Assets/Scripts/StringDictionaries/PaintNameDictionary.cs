using System;
using System.Collections.Generic;
using UnityEditor;

namespace TX_Randomizer
{
    public class PaintNameDictionary : StringDictionary<Paint.Name>
    {
#if UNITY_EDITOR
        [MenuItem("Create/StringDictionary/Paint Name Dictionary")]
        static void CreatePaintNameDictionary()
        {
            // MyClass is inheritant from ScriptableObject base class
            PaintNameDictionary example = CreateInstance<PaintNameDictionary>();
            // path has to start at "Assets"
            string path = "Assets/Scripts/ScriptableObjects/StringDictionaries/PaintName_Dictionary.asset";

            example.Dictionary = new List<EnumToStringElement<Paint.Name>>();
            Paint.Name[] paintNameValues = (Paint.Name[])Enum.GetValues(typeof(Paint.Name));

            for (int i = 0; i < paintNameValues.Length; i++)
            {
                EnumToStringElement<Paint.Name> element;
                element.KeyElement = paintNameValues[i];
                //Fix string
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