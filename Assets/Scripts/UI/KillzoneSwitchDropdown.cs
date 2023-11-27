using System.Collections.Generic;
using TMPro;

namespace TX_Randomizer
{
    public class KillzoneSwitchDropdown : DropdownBase
    {
        private void Start()
        {
            BattleInfo.OnKillzoneChanged += ApplyName;

            CreateDropdownOptionsList();
        }

        private void OnDestroy() => BattleInfo.OnKillzoneChanged -= ApplyName;

        private void ApplyName()
        {
            dropdown.value = BattleInfo.Killzone ? 1 : 0;
        }

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