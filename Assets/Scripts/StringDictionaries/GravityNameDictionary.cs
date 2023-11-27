using System;
using System.Collections.Generic;
using UnityEditor;

namespace TX_Randomizer
{
    public class GravityNameDictionary : StringDictionary<BattleInfo.GravityName>
    {
#if UNITY_EDITOR
        [MenuItem("Create/StringDictionary/Gravity Name Dictionary")]

        static void CreateNameDictionary()
        {
            // MyClass is inheritant from ScriptableObject base class
            GravityNameDictionary example = CreateInstance<GravityNameDictionary>();
            // path has to start at "Assets"
            string path = "Assets/Scripts/ScriptableObjects/StringDictionaries/GravityName_Dictionary.asset";

            example.Dictionary = new List<EnumToStringElement<BattleInfo.GravityName>>();
            BattleInfo.GravityName[] gravityNameValues = (BattleInfo.GravityName[])Enum.GetValues(typeof(BattleInfo.GravityName));

            for (int i = 0; i < gravityNameValues.Length; i++)
            {
                EnumToStringElement<BattleInfo.GravityName> element;
                element.KeyElement = gravityNameValues[i];
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