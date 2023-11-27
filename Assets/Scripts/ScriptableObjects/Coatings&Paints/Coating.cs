using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace TX_Randomizer
{
    [CreateAssetMenu(fileName = "New Coating", menuName = "Cosmetics/Coating")]
    public partial class Coating : PaintBase
    {
        public Name CoatingName;
        private const string SO_FOLDER_PATH = "Assets/Scripts/ScriptableObjects/Coatings&Paints/Coatings";
        private const string SPRITE_PREVIEW_FOLDER_PATH = "Assets/ImportedAssets/PaintsPreviews";
        private const string SUFFIX_TO_REMOVE = "_Preview.png";
        private const string DEFAULT_SPRITE_FOLDER_PATH = "Assets/ImportedAssets/TX_Images/UI";

#if UNITY_EDITOR
        [MenuItem("Create/Cosmetics/Coatings")]
        static void CreateCoatings()
        {
            Name[] coatingNameValues = (Name[])Enum.GetValues(typeof(Name));
            foreach (Name coatingName in coatingNameValues)
            {
                // Coating class is inheritant from ScriptableObject base class
                Coating instance_SO = CreateInstance<Coating>();

                // path has to start at "Assets"
                string path_SO = $"{SO_FOLDER_PATH}/{coatingName}_Coating.asset";

                instance_SO.Sprite = GetAssetSprite(coatingName);
                instance_SO.CoatingName = coatingName;

                AssetDatabase.CreateAsset(instance_SO, path_SO);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = instance_SO;
            }
        }

        private static Sprite GetAssetSprite(Name coatingName)
        {
            string[] guids = AssetDatabase.FindAssets($"{coatingName}_Preview", new[] { SPRITE_PREVIEW_FOLDER_PATH });

            if (guids.Length > 0)
            {
                foreach (string guid in guids)
                {
                    string nameToCompare = coatingName.ToString();
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
                Debug.LogWarning($"Not found {coatingName}");
                return null;
            }
            else
            {
                Debug.LogWarning($"Not found {coatingName}_Preview.png, finding Cross for replacement...");
                string[] defaultGuids = AssetDatabase.FindAssets($"Cross", new[] { DEFAULT_SPRITE_FOLDER_PATH });
                return AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(defaultGuids[0]));
            }
        }
#endif
    }
}