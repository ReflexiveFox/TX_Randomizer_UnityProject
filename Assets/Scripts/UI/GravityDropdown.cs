using System;
using System.Collections.Generic;
using TMPro;

namespace TX_Randomizer
{
    public class GravityDropdown : DropdownBase
    {
        private void Start()
        {
            BattleInfo.OnGravityChanged += ApplyGravityName;

            CreateDropdownOptionsList();
        }

        private void OnDestroy() => BattleInfo.OnGravityChanged -= ApplyGravityName;

        private void ApplyGravityName()
        {
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == TankWiki.Instance.GravityNameDictionary.GetString(BattleInfo.Gravity))
                {
                    dropdown.value = i;
                    break;
                }
            }
        }

        private void CreateDropdownOptionsList()
        {
            List<TMP_Dropdown.OptionData> options = new();

            dropdown.ClearOptions();
            foreach (BattleInfo.GravityName gravityName in (BattleInfo.GravityName[]) Enum.GetValues(typeof(BattleInfo.GravityName)))
            {
                options.Add(new TMP_Dropdown.OptionData(TankWiki.Instance.GravityNameDictionary.GetString(gravityName)));
            }
            dropdown.AddOptions(options);
        }
    }
}