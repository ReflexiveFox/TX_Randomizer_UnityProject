using System;
using System.Collections.Generic;
using TMPro;

namespace TX_Randomizer
{
    public class BattleModeDropdown : DropdownContentImage
    {
        private void Start()
        {
            BattleInfo.OnBattleModeChanged += ApplyBattleModeName;

            CreateDropdownOptionsList();
        }

        private void OnDestroy() => BattleInfo.OnBattleModeChanged -= ApplyBattleModeName;

        private void ApplyBattleModeName()
        {
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == TankWiki.Instance.BattleModeDictionary.GetString(BattleInfo.BattleMode))
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
            foreach (BattleInfo.GameMode gameMode in (BattleInfo.GameMode[]) Enum.GetValues(typeof(BattleInfo.GameMode)))
            {
                options.Add(new TMP_Dropdown.OptionData(TankWiki.Instance.BattleModeDictionary.GetString(gameMode)));
            }
            dropdown.AddOptions(options);
        }
    }
}