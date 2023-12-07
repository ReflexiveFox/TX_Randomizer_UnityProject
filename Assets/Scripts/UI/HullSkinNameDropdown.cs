#define DEBUG
#undef DEBUG

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TX_Randomizer
{
    public class HullSkinNameDropdown : DropdownBase
    {
        public static event System.Action<HullBasicInfo.Name, int> OnDropdownValueChanged = delegate { };

        [SerializeField] private bool applyTier0Skins = false;
        private HullBasicInfo.Name currentHullName = HullBasicInfo.Name.None;

        private HullSkinSlotAnimation slotAnimation;

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out slotAnimation);
        }

        private void Start()
        {
            currentHullName = HullBasicInfo.Name.None;
            HullNameDropdown.OnHullNameDropdownChange += SelectDefaultSkinFromList;     //When turret name changes, look for a skin to apply          
            dropdown.onValueChanged.AddListener(InvokeOnDropdownValueChangedEvent);     //When skin dropdown value changes, update skin image

            if (slotAnimation)
            {
                slotAnimation.OnAnimationChanging += SetHullSkin;
                slotAnimation.OnAnimationFinished += SetHullSkin;
            }
            else
            {
                Randomizer.OnHullSkinRandomized += SetHullSkin;
            }
        }

        private void OnDestroy()
        {
            HullNameDropdown.OnHullNameDropdownChange -= SelectDefaultSkinFromList;
            dropdown.onValueChanged.RemoveListener(InvokeOnDropdownValueChangedEvent);

            if (slotAnimation)
            {
                slotAnimation.OnAnimationChanging -= SetHullSkin;
                slotAnimation.OnAnimationFinished -= SetHullSkin;
            }
            else
            {
                Randomizer.OnHullSkinRandomized -= SetHullSkin;
            }
        }


        private void SetHullSkin()
        {
            HullSkin finalSkin = Randomizer.Instance.RandomizedCombo.Hull.Skin;
            List<HullSkin> hullSkinList = TankWiki.Instance.GetHullSkinsList(finalSkin.HullOwned);
            SetDropdownValue(hullSkinList.IndexOf(finalSkin));
        }

        private void SetHullSkin(HullSkin newSkin)
        {
            List<HullSkin> hullSkinList = TankWiki.Instance.GetHullSkinsList(newSkin.HullOwned);
            SetDropdownValue(hullSkinList.IndexOf(newSkin));
        }

        private void InvokeOnDropdownValueChangedEvent(int newValue)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log("HullSkinNameDropdown: InvokeOnDropdownValueChangedEvent()");
#endif
            OnDropdownValueChanged?.Invoke(currentHullName, newValue);
        }

        private void SelectDefaultSkinFromList(HullBasicInfo.Name hullName)
        {
            currentHullName = hullName;

#if DEBUG && UNITY_EDITOR
            Debug.Log($"HullSkinNameDropdown: SelectDefaultSkinFromList({hullName}) using {currentHullName}");
#endif
            CreateDropdownOptionsList(currentHullName);
            SetDropdownValue();
        }

        private void SetDropdownValue(int newValue = -1)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"HullSkinNameDropdown.SetDefaultDropdownvalue(): BEFORE Dropdown.value {dropdown.value}, applyTier0Skins {applyTier0Skins}");
#endif
            if (applyTier0Skins || currentHullName is HullBasicInfo.Name.None)
            {
                dropdown.value = 0;
                dropdown.onValueChanged.Invoke(dropdown.value);
            }
            else if (newValue == -1)     //Apply random skin option
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log($"HullSkinNameDropdown.SetDefaultDropdownvalue(): Initial Dropdown value: {dropdown.value}");
#endif
                int randomValue;
                do
                {
                    randomValue = Random.Range(0, dropdown.options.Count);
#if DEBUG && UNITY_EDITOR
                    Debug.Log($"HullSkinNameDropdown.SetDefaultDropdownvalue(): randomValue -> {randomValue}, dropdown.value -> {dropdown.value}");
#endif
                }
                while (randomValue == dropdown.value);
                dropdown.value = randomValue;
            }
            else if (newValue >= 0 && newValue < dropdown.options.Count)    //Apply selected skin option
            {
                dropdown.value = newValue;
            }
            else
            {
#if DEBUG && UNITY_EDITOR
                Debug.LogError($"{newValue} is out of range, current range: {dropdown.options.Count}");
#endif
            }

#if DEBUG && UNITY_EDITOR
            Debug.Log($"HullSkinNameDropdown: AFTER SetDefaultDropdownvalue({dropdown.value})");
#endif
        }

        private void CreateDropdownOptionsList(HullBasicInfo.Name hullName)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"HullSkinNameDropdown: CreateDropdownOptionsList({hullName})");
#endif
            dropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> options = new();
            if (hullName is HullBasicInfo.Name.None)
            {
                options.Add(new TMP_Dropdown.OptionData("None"));
            }
            else
            {
                List<HullSkin> hullSkinsList = TankWiki.Instance.GetHullSkinsList(hullName);

                foreach (HullSkin hullSkin in hullSkinsList)
                {
                    options.Add(new TMP_Dropdown.OptionData(TankWiki.Instance.SkinNameDictionary.GetString(hullSkin.GetName)));
                }

#if DEBUG && UNITY_EDITOR
                foreach (TMP_Dropdown.OptionData option in options)
                {
                    Debug.Log($"Skin option for {hullName}: {option.text}");
                }
#endif
            }
            dropdown.AddOptions(options);
        }
    }
}