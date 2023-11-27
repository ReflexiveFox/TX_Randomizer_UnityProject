using System;
using System.Collections.Generic;
using UnityEditor;

namespace TX_Randomizer
{
    public class AmmunitionNameDictionary : StringDictionary<Ammunition.Name>
    {
#if UNITY_EDITOR
        [MenuItem("Create/StringDictionary/Ammunition Name Dictionary")]
        static void CreateAmmunitionNameDictionary()
        {
            // MyClass is inheritant from ScriptableObject base class
            AmmunitionNameDictionary example = CreateInstance<AmmunitionNameDictionary>();
            // path has to start at "Assets"
            string path = "Assets/Scripts/ScriptableObjects/StringDictionaries/AmmunitionName_Dictionary.asset";

            example.Dictionary = new List<EnumToStringElement<Ammunition.Name>>();
            Ammunition.Name[] ammunitionNameValues = (Ammunition.Name[])Enum.GetValues(typeof(Ammunition.Name));

            for (int i = 0; i < ammunitionNameValues.Length; i++)
            {
                EnumToStringElement<Ammunition.Name> element;
                element.KeyElement = ammunitionNameValues[i];
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