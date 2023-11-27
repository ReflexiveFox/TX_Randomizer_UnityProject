using System;
using System.Collections.Generic;
using UnityEditor;

namespace TX_Randomizer
{
    public class BattleTimeDictionary : StringDictionary<BattleInfo.DurationTime>
    {
#if UNITY_EDITOR
        [MenuItem("Create/StringDictionary/Battle Time Dictionary")]
        static void CreateNameDictionary()
        {
            // MyClass is inheritant from ScriptableObject base class
            BattleTimeDictionary example = CreateInstance<BattleTimeDictionary>();
            // path has to start at "Assets"
            string path = "Assets/Scripts/ScriptableObjects/StringDictionaries/BattleTime_Dictionary.asset";

            example.Dictionary = new List<EnumToStringElement<BattleInfo.DurationTime>>();
            BattleInfo.DurationTime[] nameValues = (BattleInfo.DurationTime[])Enum.GetValues(typeof(BattleInfo.DurationTime));

            for (int i = 0; i < nameValues.Length; i++)
            {
                EnumToStringElement<BattleInfo.DurationTime> element;
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