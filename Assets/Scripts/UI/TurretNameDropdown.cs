#define DEBUG
#undef DEBUG

using System;
using System.Collections.Generic;
#if DEBUG && UNITY_EDITOR
using UnityEngine;
#endif

namespace TX_Randomizer
{
    public class TurretNameDropdown : DropdownBase
    {
        public static event Action<TurretBasicInfo.Name> OnTurretNameDropdownChange = delegate { };

        private TurretNameSlotAnimation slotAnimation;

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out slotAnimation);
        }

        private void Start()
        {
            dropdown.onValueChanged.AddListener(InvokeNameChangedEvent);    //Manual change and automatic
            if(slotAnimation)
            {
                //Apply first animated turrets, then the randomized combo
                slotAnimation.OnAnimationChanging += ApplyGivenTurretName;
                slotAnimation.OnAnimationFinished += ApplyFinalTurretName;
            }
            else
            {
                Randomizer.OnTurretBasicInfoRandomized += ApplyGivenTurretName;
            }
        }

        private void OnDestroy()
        {
            dropdown.onValueChanged.RemoveListener(InvokeNameChangedEvent);    //Manual change and automatic
            if (slotAnimation)
            {
                slotAnimation.OnAnimationChanging -= ApplyGivenTurretName;
                slotAnimation.OnAnimationFinished -= ApplyFinalTurretName;
            }
            else
            {
                Randomizer.OnTurretBasicInfoRandomized -= ApplyGivenTurretName;
            }
        }


        private void InvokeNameChangedEvent(int newTurretNameIndex)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"TurretNameDropdown: Invoking dropdown value changed event with {(TurretBasicInfo.Name)newTurretNameIndex}");
#endif
            OnTurretNameDropdownChange?.Invoke((TurretBasicInfo.Name)newTurretNameIndex);
        }

        private void ApplyFinalTurretName()
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"TurretNameDropdown: ApplyFinalTurretName({Randomizer.Instance.RandomizedCombo.Turret.BaseInfo.TurretName}))");
#endif
            int index = -1;
            for(int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == Randomizer.Instance.RandomizedCombo.Turret.BaseInfo.TurretName.ToString())
                {
#if DEBUG && UNITY_EDITOR
                    Debug.Log($"TurretNameDropdown.ApplyFinalTurretName(): Options[{i}] text({dropdown.options[i].text}) ==  {Randomizer.Instance.RandomizedCombo.Turret.BaseInfo.TurretName} == ");
#endif
                    index = i;
                    break;
                }
            }
            if (index < 0)
            {
#if DEBUG && UNITY_EDITOR
                Debug.LogError($"Final turret name not found");
#endif
                dropdown.value = 0;
            }
            else
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log($"TurretNameDropdown.ApplyFinalTurretName(): Final Index {index}");
#endif
                dropdown.value = index;
            }
        }


        private void ApplyGivenTurretName(TurretBasicInfo newTurretBasicInfo)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"TurretNameDropdown: ApplyGivenTurretName({newTurretBasicInfo.TurretName}({(int)newTurretBasicInfo.TurretName}))");
#endif
            if (dropdown.value != (int)newTurretBasicInfo.TurretName)
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log($"TurretNameDropdown.ApplyGivenTurretName(): dropdown.value({dropdown.value}) NOT equal to (int)newTurretBasicInfo.TurretName ({newTurretBasicInfo.TurretName}({(int)newTurretBasicInfo.TurretName}))");
#endif
                dropdown.value = (int)newTurretBasicInfo.TurretName;
            }
#if DEBUG && UNITY_EDITOR
            else
            {
                Debug.Log($"TurretNameDropdown.ApplyGivenTurretName(): dropdown.value({dropdown.value}) equal to (int)newTurretBasicInfo.TurretName ({newTurretBasicInfo.TurretName}({(int)newTurretBasicInfo.TurretName}))");
            }
#endif
        }

        private void ApplyGivenTurretName(TurretBasicInfo newTurretBasicInfo, List<TurretInfo> _)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"TurretNameDropdown: ApplyGivenTurretName({newTurretBasicInfo.TurretName}({(int)newTurretBasicInfo.TurretName}))");
#endif
            if (dropdown.value != (int)newTurretBasicInfo.TurretName)
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log($"TurretNameDropdown.ApplyGivenTurretName(): dropdown.value({dropdown.value}) NOT equal to (int)newTurretBasicInfo.TurretName ({newTurretBasicInfo.TurretName}({(int)newTurretBasicInfo.TurretName}))");
#endif
                dropdown.value = (int)newTurretBasicInfo.TurretName;
            }
#if DEBUG && UNITY_EDITOR
            else
            {
                Debug.Log($"TurretNameDropdown.ApplyGivenTurretName(): dropdown.value({dropdown.value}) equal to (int)newTurretBasicInfo.TurretName ({newTurretBasicInfo.TurretName}({(int)newTurretBasicInfo.TurretName}))");
            }
#endif
        }
    }
}