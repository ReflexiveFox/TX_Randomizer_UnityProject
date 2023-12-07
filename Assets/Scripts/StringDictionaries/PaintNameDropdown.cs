using System.Collections.Generic;
using TMPro;

namespace TX_Randomizer
{
    public class PaintNameDropdown : DropdownBase
    {
        PaintSlotAnimation slotAnimation;

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out slotAnimation);
        }

        private void Start()
        {
            if (slotAnimation)
            {
                slotAnimation.OnAnimationChanging += ApplyPaintName;
                slotAnimation.OnAnimationFinished += ApplyPaintName;
            }
            else
            {
                PlayerComboHandler.Instance.PlayerCombo.OnPaintChanged += ApplyPaintName;
            }
            
            dropdown.onValueChanged.AddListener(ApplyPaintSprite);
            CreateDropdownOptionsList();
        }

        private void OnDestroy()
        {
            if (slotAnimation)
            {
                slotAnimation.OnAnimationChanging -= ApplyPaintName;
                slotAnimation.OnAnimationFinished -= ApplyPaintName;
            }
            else
            {
                PlayerComboHandler.Instance.PlayerCombo.OnPaintChanged -= ApplyPaintName;
            }
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
                if (dropdown.options[i].text == TankWiki.Instance.PaintNameDictionary.GetString(Randomizer.Instance.RandomizedCombo.Paint.PaintName))
                {
                    dropdown.value = i;
                    break;
                }
            }
        }

        private void ApplyPaintName(Paint newPaint)
        {
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == TankWiki.Instance.PaintNameDictionary.GetString(newPaint.PaintName))
                {
                    dropdown.value = i;
                    break;
                }
            }
        }

        private void CreateDropdownOptionsList()
        {
            List<TMP_Dropdown.OptionData> options = new();
            List<Paint> currentPaints = new List<Paint>(TankWiki.Instance.Paints);
            dropdown.ClearOptions();

            for (int i = 0; i < currentPaints.Count; i++)
            {
                TMP_Dropdown.OptionData option = new(TankWiki.Instance.PaintNameDictionary.GetString(currentPaints[i].PaintName), currentPaints[i].Sprite);
                options.Add(option);
            }
            dropdown.AddOptions(options);
        }
    }
}