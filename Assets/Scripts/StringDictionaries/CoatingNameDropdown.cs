using System.Collections.Generic;
using TMPro;

namespace TX_Randomizer
{
    public class CoatingNameDropdown : DropdownBase
    {
        private CoatingSlotAnimation slotAnimation;

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out slotAnimation);
        }

        private void Start()
        {        
            dropdown.onValueChanged.AddListener(ApplyCoatingSprite);
            CreateDropdownOptionsList();
            if(slotAnimation)
            {
                slotAnimation.OnAnimationChanging += ApplyCoatingName;
                slotAnimation.OnAnimationFinished += ApplyCoatingName;
            }
            else
            {
                PlayerComboHandler.Instance.PlayerCombo.OnCoatingChanged += ApplyCoatingName;
            }
        }

        private void OnDestroy()
        {         
            dropdown.onValueChanged.RemoveListener(ApplyCoatingSprite);
            if (slotAnimation)
            {
                slotAnimation.OnAnimationChanging -= ApplyCoatingName;
                slotAnimation.OnAnimationFinished -= ApplyCoatingName;
            }
            else
            {
                PlayerComboHandler.Instance.PlayerCombo.OnCoatingChanged -= ApplyCoatingName;
            }
        }

        private void ApplyCoatingSprite(int coatingIndex)
        {
            dropdown.captionImage.sprite = dropdown.options[coatingIndex].image;
        }

        private void ApplyCoatingName()
        {

            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == TankWiki.Instance.CoatingNameDictionary.GetString(Randomizer.Instance.RandomizedCombo.Coating.CoatingName))
                {
                    dropdown.value = i;
                    break;
                }
            }
        }

        private void ApplyCoatingName(Coating newCoating)
        {

            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == TankWiki.Instance.CoatingNameDictionary.GetString(newCoating.CoatingName))
                {
                    dropdown.value = i;
                    break;
                }
            }
        }

        private void CreateDropdownOptionsList()
        {
            List<TMP_Dropdown.OptionData> options = new();
            List<Coating> currentCoatings = new List<Coating>(TankWiki.Instance.Coatings);
            dropdown.ClearOptions();
           
            for (int i = 0; i < currentCoatings.Count; i++)
            {
                TMP_Dropdown.OptionData option = new(TankWiki.Instance.CoatingNameDictionary.GetString(currentCoatings[i].CoatingName), currentCoatings[i].Sprite);
                options.Add(option);
            }
            dropdown.AddOptions(options);
        }
    }
}