using System;
using System.Collections.Generic;
using UnityEditor;

namespace TX_Randomizer
{
    public class BattleModeDictionary : StringDictionary<BattleInfo.GameMode>
    {
#if UNITY_EDITOR
        [MenuItem("Create/StringDictionary/Battle Mode Dictionary")]
        static void CreateNameDictionary()
        {
            // MyClass is inheritant from ScriptableObject base class
            BattleModeDictionary example = CreateInstance<BattleModeDictionary>();
            // path has to start at "Assets"
            string path = "Assets/Scripts/ScriptableObjects/StringDictionaries/BattleMode_Dictionary.asset";

            example.Dictionary = new List<EnumToStringElement<BattleInfo.GameMode>>();
            BattleInfo.GameMode[] nameValues = (BattleInfo.GameMode[])Enum.GetValues(typeof(BattleInfo.GameMode));

            for (int i = 0; i < nameValues.Length; i++)
            {
                EnumToStringElement<BattleInfo.GameMode> element;
                element.KeyElement = nameValues[i];
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