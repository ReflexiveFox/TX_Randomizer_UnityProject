#define DEBUG
#undef DEBUG

using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

namespace TX_Randomizer
{
    public class AmmunitionNameDropdown : DropdownBase
    {
        public static event Action<TurretBasicInfo.Name, int> OnDropdownValueChanged = delegate { };
        
        [SerializeField] private bool applyDefaultAmmo = false;

        private AmmunitionListContainer _currentAmmoContainer;

        private AmmunitionSlotAnimation slotAnimation;

        private TurretBasicInfo.Name CurrentTurretName
        {
            get => _currentAmmoContainer.TurretOwned;
            set => _currentAmmoContainer = TankWiki.Instance.GetAmmunitionListContainer(value);
        }

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out slotAnimation);
        }

        private void Start()
        {
            CurrentTurretName = TurretBasicInfo.Name.None;
            TurretNameDropdown.OnTurretNameDropdownChange += SelectDefaultAmmunitionFromList;     //When turret name changes, look for a skin to apply          
            dropdown.onValueChanged.AddListener(InvokeAmmunitionChangedEvent);

            if (slotAnimation)
            {
                slotAnimation.OnAnimationChanging += SetAmmunitionSkin;
                slotAnimation.OnAnimationFinished += SetAmmunitionSkin;
            }
            else
            {
                Randomizer.OnAmmunitionSlotRandomized += SetAmmunitionSkin;
            }

            //Randomizer.OnAmmunitionSlotRandomized += ApplyAmmunitionName;
        }

        private void OnDestroy()
        {
            TurretNameDropdown.OnTurretNameDropdownChange -= SelectDefaultAmmunitionFromList;     //When turret name changes, look for a skin to apply          
            dropdown.onValueChanged.RemoveListener(InvokeAmmunitionChangedEvent);

            if (slotAnimation)
            {
                slotAnimation.OnAnimationChanging -= SetAmmunitionSkin;
                slotAnimation.OnAnimationFinished -= SetAmmunitionSkin;
            }
            else
            {
                Randomizer.OnAmmunitionSlotRandomized -= SetAmmunitionSkin;
            }
        }

        private void SelectDefaultAmmunitionFromList(TurretBasicInfo.Name turretName)
        {
            CurrentTurretName = turretName;
#if DEBUG && UNITY_EDITOR
            Debug.Log($"AmmunitionNameDropdown: SelectDefaultAmmunitionFromList({turretName}) using {CurrentTurretName}");
#endif
            CreateDropdownOptionsList(_currentAmmoContainer.AmmunitionsList);
            SetDropdownValue();
        }

        private void CreateDropdownOptionsList(List<Ammunition> ammoList)
        {
            List<TMP_Dropdown.OptionData> options = new();

            dropdown.ClearOptions();
            foreach (Ammunition ammunition in ammoList)
            {
                options.Add(new TMP_Dropdown.OptionData(ammunition.AmmoName.ToString()));
            }
            dropdown.AddOptions(options);
        }

        private void InvokeAmmunitionChangedEvent(int newAmmunitionIndex)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"AmmunitionNameDropdown: Invoking dropdown value changed event with {(Ammunition.Name)newAmmunitionIndex}");
#endif
            OnDropdownValueChanged?.Invoke(CurrentTurretName, newAmmunitionIndex);
        }

        private void SetAmmunitionSkin()
        {
            List<Ammunition> ammunitionList = TankWiki.Instance.GetAmmunitionListOf(Randomizer.Instance.RandomizedCombo.Turret.Ammunition.TurretOwned);
            SetDropdownValue(ammunitionList.IndexOf(Randomizer.Instance.RandomizedCombo.Turret.Ammunition));
        }

        private void SetAmmunitionSkin(Ammunition ammunition)
        {
            List<Ammunition> ammunitionList = TankWiki.Instance.GetAmmunitionListOf(ammunition.TurretOwned);
            SetDropdownValue(ammunitionList.IndexOf(ammunition));
        }

        private void SetAmmunitionSkin(AmmunitionListContainer container, Ammunition newAmmunition)
        {
            List<Ammunition> ammunitionList = container.AmmunitionsList;
            SetDropdownValue(ammunitionList.IndexOf(newAmmunition));
        }

        private void SetDropdownValue(int newValue = -1)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"AmmunitionSkinNameDropdown.SetDefaultDropdownvalue(): BEFORE Dropdown.value {dropdown.value}, applyDefaultAmmo {applyDefaultAmmo}");
#endif
            if(applyDefaultAmmo || CurrentTurretName == TurretBasicInfo.Name.None)
            {
                dropdown.value = 0;
                dropdown.onValueChanged.Invoke(dropdown.value);
            }
            else if (newValue == -1)     //Apply random skin option
            {
                int randomValue;
                do
                {
                    randomValue = Random.Range(0, dropdown.options.Count);
#if DEBUG && UNITY_EDITOR
                    Debug.Log($"AmmunitionNameDropdown.SetDefaultDropdownvalue(): randomValue -> {randomValue}, dropdown.value -> {dropdown.value}");
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
            Debug.Log($"AmmunitionSkinNameDropdown: AFTER SetDefaultDropdownvalue({dropdown.value})");
#endif
        }
    }
}