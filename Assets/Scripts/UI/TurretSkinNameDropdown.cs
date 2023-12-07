#define DEBUG
#undef DEBUG

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TX_Randomizer
{
    public class TurretSkinNameDropdown : DropdownBase
    {
        public static event System.Action<TurretBasicInfo.Name, int> OnDropdownValueChanged = delegate { };

        [SerializeField] private bool applyTier0Skins = false;
        private TurretBasicInfo.Name currentTurretName = TurretBasicInfo.Name.None;

        private TurretSkinSlotAnimation slotAnimation;

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out slotAnimation);
        }

        private void Start()
        {
            currentTurretName = TurretBasicInfo.Name.None;
            TurretNameDropdown.OnTurretNameDropdownChange += SelectDefaultSkinFromList;     //When turret name changes, look for a skin to apply          
            dropdown.onValueChanged.AddListener(InvokeOnDropdownValueChangedEvent);     //When skin dropdown value changes, update skin image
            
            if (slotAnimation)
            {
                slotAnimation.OnAnimationChanging += SetTurretSkin;
                slotAnimation.OnAnimationFinished += SetTurretSkin;
            }
            else
            {
                Randomizer.OnTurretSkinRandomized += SetTurretSkin;
            }
        }

        private void OnDestroy()
        {
            TurretNameDropdown.OnTurretNameDropdownChange -= SelectDefaultSkinFromList;          
            dropdown.onValueChanged.RemoveListener(InvokeOnDropdownValueChangedEvent);
            
            if (slotAnimation)
            {
                slotAnimation.OnAnimationChanging -= SetTurretSkin;
                slotAnimation.OnAnimationFinished -= SetTurretSkin;
            }
            else
            {
                Randomizer.OnTurretSkinRandomized -= SetTurretSkin;
            }
        }

        private void SetTurretSkin()
        {
            TurretSkin finalSkin = Randomizer.Instance.RandomizedCombo.Turret.Skin;
            List<TurretSkin> turretSkinList = TankWiki.Instance.GetTurretSkinsList(finalSkin.TurretOwned);
            SetDropdownValue(turretSkinList.IndexOf(finalSkin));
        }

        private void SetTurretSkin(TurretSkin newSkin)
        {
            List<TurretSkin> turretSkinList = TankWiki.Instance.GetTurretSkinsList(newSkin.TurretOwned);
            SetDropdownValue(turretSkinList.IndexOf(newSkin));
        }

        private void InvokeOnDropdownValueChangedEvent(int newValue)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log("TurretSkinNameDropdown: InvokeOnDropdownValueChangedEvent()");
#endif
            OnDropdownValueChanged?.Invoke(currentTurretName, newValue);
        }

        private void SelectDefaultSkinFromList(TurretBasicInfo.Name turretName)
        {
            currentTurretName = turretName;
            
#if DEBUG && UNITY_EDITOR
            Debug.Log($"TurretSkinNameDropdown: SelectDefaultSkinFromList({turretName}) using {currentTurretName}");
#endif
            CreateDropdownOptionsList(currentTurretName);
            SetDropdownValue();
        }

        private void SetDropdownValue(int newValue = -1)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"TurretSkinNameDropdown.SetDefaultDropdownvalue(): BEFORE Dropdown.value {dropdown.value}, applyTier0Skins {applyTier0Skins}");
#endif
            if (applyTier0Skins || currentTurretName is TurretBasicInfo.Name.None)
            {
                dropdown.value = 0;
                dropdown.onValueChanged.Invoke(dropdown.value);
            }
            else if(newValue == -1)     //Apply random skin option
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log($"TurretSkinNameDropdown.SetDefaultDropdownvalue(): Initial Dropdown value: {dropdown.value}");
#endif
                int randomValue;
                do
                {
                    randomValue = Random.Range(0, dropdown.options.Count);
#if DEBUG && UNITY_EDITOR
                    Debug.Log($"TurretSkinNameDropdown.SetDefaultDropdownvalue(): randomValue -> {randomValue}, dropdown.value -> {dropdown.value}");
#endif
                } 
                while (randomValue == dropdown.value);
                dropdown.value = randomValue;
            }
            else if(newValue >= 0 && newValue < dropdown.options.Count)    //Apply selected skin option
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
            Debug.Log($"TurretSkinNameDropdown: AFTER SetDefaultDropdownvalue({dropdown.value})");
#endif
        } 

        private void CreateDropdownOptionsList(TurretBasicInfo.Name turretName)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"TurretSkinNameDropdown: CreateDropdownOptionsList({turretName})");
#endif
            dropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> options = new();
            if (turretName is TurretBasicInfo.Name.None)
            {
                options.Add(new TMP_Dropdown.OptionData("None"));
            }
            else
            {
                List<TurretSkin> turretSkinsList = TankWiki.Instance.GetTurretSkinsList(turretName);

                foreach (TurretSkin turretSkin in turretSkinsList)
                {
                    options.Add(new TMP_Dropdown.OptionData(TankWiki.Instance.SkinNameDictionary.GetString(turretSkin.GetName)));
                }
                
#if DEBUG && UNITY_EDITOR
                foreach (TMP_Dropdown.OptionData option in options)
                {
                    Debug.Log($"Skin option for {turretName}: {option.text}");
                }
#endif
            }
            dropdown.AddOptions(options);
        }
    }
}