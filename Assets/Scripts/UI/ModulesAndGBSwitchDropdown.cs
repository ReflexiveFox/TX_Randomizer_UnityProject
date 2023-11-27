using System.Collections.Generic;
using TMPro;

namespace TX_Randomizer
{
    public class ModulesAndGBSwitchDropdown : DropdownBase
    {
        private void Start()
        {           
            BattleInfo.OnModulesAndGB_Changed += ApplyName;

            CreateDropdownOptionsList();
        }

        private void OnDestroy() => BattleInfo.OnModulesAndGB_Changed -= ApplyName;

        private void ApplyName() => dropdown.value = BattleInfo.ModulesAndGB ? 1 : 0;

        private void CreateDropdownOptionsList()
        {
            List<TMP_Dropdown.OptionData> options = new();

            dropdown.ClearOptions();
            options.Add(new TMP_Dropdown.OptionData("Off"));
            options.Add(new TMP_Dropdown.OptionData("On"));
            dropdown.AddOptions(options);
        }
    }
}