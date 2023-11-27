using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TX_Randomizer
{
    public class HullSkinNameDropdown : DropdownContentImage
    {
        [SerializeField] private TMP_Dropdown hullNameDropdown;

        private void Start()
        {
            Randomizer.OnHullSkinRandomized += ApplyHullSkinName;
            dropdown.onValueChanged.AddListener(ApplyHullSkinSprite);
            hullNameDropdown.onValueChanged.AddListener(ApplyDefaultSkin);
        }

        private void OnDestroy()
        {
            Randomizer.OnHullSkinRandomized -= ApplyHullSkinName;
            dropdown.onValueChanged.RemoveListener(ApplyHullSkinSprite);
            hullNameDropdown.onValueChanged.RemoveListener(CreateDropdownOptionsList);
        }

        private void ApplyHullSkinSprite(int hullSkinIndex)
        {
            List<HullSkin> hullSkinsList = TankWiki.instance.GetHullSkinsList((HullBasicInfo.Name)hullNameDropdown.value);
            contentImage.sprite = hullSkinsList[dropdown.value].Sprite;
        }

        private void ApplyHullSkinName(HullSkin newHullSkin)
        {
            CreateDropdownOptionsList(hullNameDropdown.value);
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == TankWiki.instance.SkinNameDictionary.GetString(newHullSkin.GetName))
                {
                    dropdown.value = i;
                    ApplyHullSkinSprite(i);
                    break;
                }
            }
        }

        private void ApplyDefaultSkin(int hullNameIndex)
        {
            CreateDropdownOptionsList(hullNameIndex);
            ApplyHullSkinSprite(0);
        }

        private void CreateDropdownOptionsList(int hullNameIndex)
        {        
            List<TMP_Dropdown.OptionData> options = new();
            List<HullSkin> hullSkinsList = TankWiki.instance.GetHullSkinsList((HullBasicInfo.Name)hullNameIndex);
         
            dropdown.ClearOptions();
            foreach (HullSkin hullSkin in hullSkinsList)
            {
                options.Add(new TMP_Dropdown.OptionData(TankWiki.instance.SkinNameDictionary.GetString(hullSkin.GetName)));
            }
            dropdown.AddOptions(options);
        }
    }
}