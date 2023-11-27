using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TX_Randomizer
{
    public class TurretSkinNameDropdown : DropdownContentImage
    {
        [SerializeField] private TMP_Dropdown turretNameDropdown;

        private void Start()
        {
            Randomizer.OnTurretSkinRandomized += ApplyTurretSkinName;
            dropdown.onValueChanged.AddListener(ApplyTurretSkinSprite);
            turretNameDropdown.onValueChanged.AddListener(ApplyDefaultSkin);
        }

        private void OnDestroy()
        {
            Randomizer.OnTurretSkinRandomized -= ApplyTurretSkinName;
            dropdown.onValueChanged.RemoveListener(ApplyTurretSkinSprite);
            turretNameDropdown.onValueChanged.RemoveListener(CreateDropdownOptionsList);
        }

        private void ApplyTurretSkinSprite(int turretSkinIndex)
        {
            List<TurretSkin> turretSkinsList = TankWiki.instance.GetTurretSkinsList((TurretBasicInfo.Name)turretNameDropdown.value);
            contentImage.sprite = turretSkinsList[dropdown.value].Sprite;
        }

        private void ApplyTurretSkinName(TurretSkin newTurretSkin)
        {
            CreateDropdownOptionsList(turretNameDropdown.value);
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == TankWiki.instance.SkinNameDictionary.GetString(newTurretSkin.GetName))
                {
                    dropdown.value = i;
                    ApplyTurretSkinSprite(i);
                    break;
                }
            }
        }

        private void ApplyDefaultSkin(int turretNameIndex)
        {
            CreateDropdownOptionsList(turretNameIndex);
            ApplyTurretSkinSprite(0);
        }

        private void CreateDropdownOptionsList(int turretNameIndex)
        {        
            List<TMP_Dropdown.OptionData> options = new();
            List<TurretSkin> turretSkinsList = TankWiki.instance.GetTurretSkinsList((TurretBasicInfo.Name)turretNameIndex);
         
            dropdown.ClearOptions();
            foreach (TurretSkin turretSkin in turretSkinsList)
            {
                options.Add(new TMP_Dropdown.OptionData(TankWiki.instance.SkinNameDictionary.GetString(turretSkin.GetName)));
            }
            dropdown.AddOptions(options);
        }
    }
}