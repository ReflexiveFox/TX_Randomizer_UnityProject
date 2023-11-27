using System.Collections.Generic;
using TMPro;

namespace TX_Randomizer
{
    public class FriendlyFireSwitchDropdown : DropdownBase
    {
        private void Start()
        {
            BattleInfo.OnFriendlyFireChanged += ApplyFriendlyFireName;

            CreateDropdownOptionsList();
        }

        private void OnDestroy() => BattleInfo.OnFriendlyFireChanged -= ApplyFriendlyFireName;

        private void ApplyFriendlyFireName() => dropdown.value = BattleInfo.FriendlyFire ? 1 : 0;

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