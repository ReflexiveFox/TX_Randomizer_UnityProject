using UnityEditor;
using System.Collections.Generic;
using System;

namespace TX_Randomizer
{
    public class SkinNameDictionary : StringDictionary<Skin.Name>
    {
#if UNITY_EDITOR
        [MenuItem("Create/StringDictionary/Skin Name Dictionary")]
        
        static void CreateSkinNameDictionary()
        {
            // MyClass is inheritant from ScriptableObject base class
            SkinNameDictionary example = CreateInstance<SkinNameDictionary>();
            // path has to start at "Assets"
            string path = "Assets/Scripts/ScriptableObjects/StringDictionaries/SkinName_Dictionary.asset";
            
            example.Dictionary = new List<EnumToStringElement<Skin.Name>>();
            Skin.Name[] skinNameValues = (Skin.Name[]) Enum.GetValues(typeof(Skin.Name));

            for (int i = 0; i < skinNameValues.Length; i++)
            {
                EnumToStringElement<Skin.Name> element;
                element.KeyElement = skinNameValues[i];
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