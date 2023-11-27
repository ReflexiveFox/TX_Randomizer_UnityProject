using System.Collections.Generic;
using TMPro;

namespace TX_Randomizer
{
    public class CoatingNameDropdown : DropdownBase
    {
        private void Start()
        {
            PlayerCombo.Turret.OnCoatingChanged += ApplyCoatingName;
            dropdown.onValueChanged.AddListener(ApplyCoatingSprite);
            CreateDropdownOptionsList();
        }

        private void OnDestroy()
        {
            PlayerCombo.Turret.OnCoatingChanged -= ApplyCoatingName;
            dropdown.onValueChanged.RemoveListener(ApplyCoatingSprite);
        }

        private void ApplyCoatingSprite(int coatingIndex)
        {
            dropdown.captionImage.sprite = dropdown.options[coatingIndex].image;
        }

        private void ApplyCoatingName()
        {
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == TankWiki.instance.CoatingNameDictionary.GetString(PlayerCombo.Turret.Coating.CoatingName))
                {
                    dropdown.value = i;
                    break;
                }
            }
        }

        private void CreateDropdownOptionsList()
        {
            List<TMP_Dropdown.OptionData> options = new();
            List<Coating> currentCoatings = new List<Coating>(TankWiki.instance.Coatings);
            dropdown.ClearOptions();
           
            for (int i = 0; i < currentCoatings.Count; i++)
            {
                TMP_Dropdown.OptionData option = new(TankWiki.instance.CoatingNameDictionary.GetString(currentCoatings[i].CoatingName), currentCoatings[i].Sprite);
                options.Add(option);
            }
            dropdown.AddOptions(options);
        }
    }
}