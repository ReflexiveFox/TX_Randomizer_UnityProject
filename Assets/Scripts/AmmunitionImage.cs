#define DEBUG
#undef DEBUG

using System.Collections.Generic;
using UnityEngine;

namespace TX_Randomizer
{
    public class AmmunitionImage : SkinImage
    {
        [SerializeField] private AmmunitionNameDropdown ammoDropdown;

        private void Start()
        {
            AmmunitionNameDropdown.OnDropdownValueChanged += ApplyAmmunitionSprite;
        }

        private void OnDestroy()
        {
            AmmunitionNameDropdown.OnDropdownValueChanged -= ApplyAmmunitionSprite;
        }

        private void ApplyAmmunitionSprite(TurretBasicInfo.Name turretName, int ammoIndex)
        {
            List<Ammunition> ammunitionList = TankWiki.Instance.GetAmmunitionListOf(turretName);
            Ammunition ammunition = ammunitionList[ammoIndex];
#if DEBUG && UNITY_EDITOR
            Debug.Log($"AmmoSkinImage: Changing sprite to {ammunition.AmmoName}({ammoIndex}) of {turretName}");
#endif
            skinImage.sprite = ammunition.Sprite;
        }
    }
}