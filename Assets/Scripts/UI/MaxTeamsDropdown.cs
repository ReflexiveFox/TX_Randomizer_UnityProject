using System.Collections.Generic;
using TMPro;

namespace TX_Randomizer
{
    public class MaxTeamsDropdown : DropdownBase
    {
        private void Start()
        {
            BattleInfo.OnMaxTeamsChanged += ApplyMaxTeamsName;

            CreateDropdownOptionsList();
        }

        private void OnDestroy() => BattleInfo.OnMaxTeamsChanged -= ApplyMaxTeamsName;

        private void ApplyMaxTeamsName()
        {
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == BattleInfo.MaxPlayers.ToString())
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
            for (int i = 1; i <= TankWiki.MAX_PLAYERS; i++)
            {
                options.Add(new TMP_Dropdown.OptionData(i.ToString()));
            }
            dropdown.AddOptions(options);
        }
    }
}