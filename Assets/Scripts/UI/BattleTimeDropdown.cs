using System;
using System.Collections.Generic;
using TMPro;

namespace TX_Randomizer
{
    public class BattleTimeDropdown : DropdownBase
    {
        private void Start()
        {
            BattleInfo.OnBattleTimeChanged += ApplyBattleTimeName;

            CreateDropdownOptionsList();
        }

        private void OnDestroy() => BattleInfo.OnBattleTimeChanged -= ApplyBattleTimeName;

        private void ApplyBattleTimeName()
        {
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == TankWiki.instance.BattleTimeDictionary.GetString(BattleInfo.BattleTime))
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
            foreach (BattleInfo.DurationTime battleTime in (BattleInfo.DurationTime[]) Enum.GetValues(typeof(BattleInfo.DurationTime)))
            {
                options.Add(new TMP_Dropdown.OptionData(TankWiki.instance.BattleTimeDictionary.GetString(battleTime)));
            }
            dropdown.AddOptions(options);
        }
    }
}