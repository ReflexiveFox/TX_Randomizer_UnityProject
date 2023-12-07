#define DEBUG
#undef DEBUG

using UnityEngine;

namespace TX_Randomizer
{
    public class HullSkinImage : SkinImage
    {
        [SerializeField] private HullSkinNameDropdown skinDropdown;

        private void Start()
        {
            HullSkinNameDropdown.OnDropdownValueChanged += ApplySkinSprite;
        }

        private void OnDestroy()
        {
            HullSkinNameDropdown.OnDropdownValueChanged -= ApplySkinSprite;
        }

        private void ApplySkinSprite(HullBasicInfo.Name hullName, int skinIndex)
        {
            if (hullName == HullBasicInfo.Name.None)
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log($"HullSkinImage: Changing sprite to {hullName}({skinIndex}) of {hullName}");
#endif
                skinImage.sprite = noneSprite;
            }
            else
            {
                HullSkin hullSkin = TankWiki.Instance.GetHullSkin(hullName, skinIndex);
#if DEBUG && UNITY_EDITOR
            Debug.Log($"HullSkinImage: Changing sprite to {hullSkin.GetName}({skinIndex}) of {hullName}");
#endif
                skinImage.sprite = hullSkin.Sprite;
            }
        }
    }
}