#define DEBUG
#undef DEBUG

using UnityEngine;

namespace TX_Randomizer
{
    public class TurretSkinImage : SkinImage
    {
        [SerializeField] private TurretSkinNameDropdown skinDropdown;

        private void Start()
        {
            TurretSkinNameDropdown.OnDropdownValueChanged += ApplySkinSprite;
        }

        private void OnDestroy()
        {
            TurretSkinNameDropdown.OnDropdownValueChanged -= ApplySkinSprite;
        }

        private void ApplySkinSprite(TurretBasicInfo.Name turretName, int skinIndex)
        {
            if(turretName == TurretBasicInfo.Name.None)
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log($"TurretSkinImage: Changing sprite to {turretName}({skinIndex}) of {turretName}");
#endif
                skinImage.sprite = noneSprite;
            }
            else
            {
                TurretSkin turretSkin = TankWiki.Instance.GetTurretSkin(turretName, skinIndex);
#if DEBUG && UNITY_EDITOR
            Debug.Log($"TurretSkinImage: Changing sprite to {turretSkin.GetName}({skinIndex}) of {turretName}");
#endif
                skinImage.sprite = turretSkin.Sprite;
            }
        }
    }
}