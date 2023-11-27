using System;
using System.Collections.Generic;
using UnityEditor;

namespace TX_Randomizer
{
    public class CoatingNameDictionary : StringDictionary<Coating.Name>
    {
#if UNITY_EDITOR
        [MenuItem("Create/StringDictionary/Coating Name Dictionary")]
        static void CreateCoatingNameDictionary()
        {
            // MyClass is inheritant from ScriptableObject base class
            CoatingNameDictionary example = CreateInstance<CoatingNameDictionary>();
            // path has to start at "Assets"
            string path = "Assets/Scripts/ScriptableObjects/StringDictionaries/CoatingName_Dictionary.asset";

            example.Dictionary = new List<EnumToStringElement<Coating.Name>>();
            Coating.Name[] paintNameValues = (Coating.Name[])Enum.GetValues(typeof(Coating.Name));

            for (int i = 0; i < paintNameValues.Length; i++)
            {
                EnumToStringElement<Coating.Name> element;
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