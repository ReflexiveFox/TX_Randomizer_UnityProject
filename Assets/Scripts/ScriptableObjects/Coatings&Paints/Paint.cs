using System;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

namespace TX_Randomizer
{
    [CreateAssetMenu(fileName = "_Paint", menuName = "Cosmetics/Paint")]
    public partial class Paint : PaintBase
    {
        public Name PaintName;
        private const string SO_FOLDER_PATH = "Assets/Scripts/ScriptableObjects/Coatings&Paints/Paints";
        private const string SPRITE_PREVIEW_FOLDER_PATH = "Assets/ImportedAssets/PaintsPreviews";
        private const string SUFFIX_TO_REMOVE = "_Preview.png";
        private const string DEFAULT_SPRITE_FOLDER_PATH = "Assets/ImportedAssets/TX_Images/UI";

#if UNITY_EDITOR
        [MenuItem("Create/Cosmetics/Paints")]
        static void CreatePaints()
        {
            Name[] paintNameValues = (Name[])Enum.GetValues(typeof(Name));
            foreach (Name paintName in paintNameValues)
            {
                // Coating class is inheritant from ScriptableObject base class
                Paint instance_SO = CreateInstance<Paint>();

                // path has to start at "Assets"
                string path_SO = $"{SO_FOLDER_PATH}/{paintName}_Paint.asset";

                instance_SO.Sprite = GetAssetSprite(paintName);
                instance_SO.PaintName = paintName;

                AssetDatabase.CreateAsset(instance_SO, path_SO);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = instance_SO;
            }
        }

        private static Sprite GetAssetSprite(Name paintName)
        {
            string[] guids = AssetDatabase.FindAssets($"{paintName}_Preview", new[] { SPRITE_PREVIEW_FOLDER_PATH });

            if (guids.Length > 0)
            {
                foreach (string guid in guids)
                {
                    string nameToCompare = paintName.ToString();
                    string spriteAssetPath = AssetDatabase.GUIDToAssetPath(guid);
                    string fullFileName = Path.GetFileName(spriteAssetPath);
                    if (fullFileName.Contains(SUFFIX_TO_REMOVE))
                    {
                        string partialFileName = fullFileName.Substring(0, fullFileName.Length - (SUFFIX_TO_REMOVE.Length));
                        Debug.Log($"Checking if {partialFileName} == {nameToCompare}");
                        if (partialFileName == nameToCompare)
                        {
                            return AssetDatabase.LoadAssetAtPath<Sprite>(spriteAssetPath);
                        }
                    }
                }
                Debug.LogWarning($"Not found {paintName}");
                return null;
            }
            else
            {
                Debug.LogWarning($"Not found {paintName}_Preview.png, finding Cross for replacement...");
                string[] defaultGuids = AssetDatabase.FindAssets($"Cross", new[] { DEFAULT_SPRITE_FOLDER_PATH });
                return AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(defaultGuids[0]));
            }
        }
#endif
    }
}