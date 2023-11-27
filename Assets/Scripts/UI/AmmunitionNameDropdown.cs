using System.Collections.Generic;
using TMPro;

namespace TX_Randomizer
{
    public class AmmunitionNameDropdown : DropdownContentImage
    {
        private List<Ammunition> currentAmmoList;

        private void Start()
        {
            Randomizer.OnAmmunitionSlotRandomized += ApplyAmmunitionName;
            dropdown.onValueChanged.AddListener(ApplyAmmunitionSprite);
        }

        private void OnDestroy()
        {
            Randomizer.OnAmmunitionSlotRandomized -= ApplyAmmunitionName;
            dropdown.onValueChanged.RemoveListener(ApplyAmmunitionSprite);
        }

        private void ApplyAmmunitionSprite(int ammoIndex)
        {
            foreach(Ammunition ammo in currentAmmoList)
            {
                if (ammo.AmmoName.ToString() == dropdown.captionText.text)
                {
                    contentImage.sprite = ammo.Sprite;
                }
            }
        }

        private void ApplyAmmunitionName(AmmunitionListContainer ammoListContainer, Ammunition newAmmo)
        {
            currentAmmoList = new List<Ammunition>(ammoListContainer.AmmunitionsList);
            CreateDropdownOptionsList(ammoListContainer.AmmunitionsList);
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == newAmmo.AmmoName.ToString())
                {
                    dropdown.value = i;
                    ApplyAmmunitionSprite(i);
                    break;
                }
            }  
        }

        private void CreateDropdownOptionsList(List<Ammunition> ammoList)
        {
            List<TMP_Dropdown.OptionData> options = new();

            dropdown.ClearOptions();
            foreach (Ammunition ammunition in ammoList)
            {
                options.Add(new TMP_Dropdown.OptionData(ammunition.AmmoName.ToString()));
            }
            dropdown.AddOptions(options);
        }
    }
}