using System.Collections.Generic;
using TMPro;

namespace TX_Randomizer
{
    public class PaintNameDropdown : DropdownBase
    {
        private void Start()
        {
            PlayerCombo.Hull.OnPaintChanged += ApplyPaintName;
            dropdown.onValueChanged.AddListener(ApplyPaintSprite);
            CreateDropdownOptionsList();
        }

        private void OnDestroy()
        {
            PlayerCombo.Hull.OnPaintChanged -= ApplyPaintName;
            dropdown.onValueChanged.RemoveListener(ApplyPaintSprite);
        }

        private void ApplyPaintSprite(int paintIndex)
        {
            dropdown.captionImage.sprite = dropdown.options[paintIndex].image;
        }

        private void ApplyPaintName()
        {
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == TankWiki.instance.PaintNameDictionary.GetString(PlayerCombo.Hull.Paint.PaintName))
                {
                    dropdown.value = i;
                    break;
                }
            }
        }

        private void CreateDropdownOptionsList()
        {
            List<TMP_Dropdown.OptionData> options = new();
            List<Paint> currentPaints = new List<Paint>(TankWiki.instance.Paints);
            dropdown.ClearOptions();

            for (int i = 0; i < currentPaints.Count; i++)
            {
                TMP_Dropdown.OptionData option = new(TankWiki.instance.PaintNameDictionary.GetString(currentPaints[i].PaintName), currentPaints[i].Sprite);
                options.Add(option);
            }
            dropdown.AddOptions(options);
        }
    }
}