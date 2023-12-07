#define DEBUG
#undef DEBUG

using System;
using System.Collections.Generic;
#if DEBUG && UNITY_EDITOR
using UnityEngine;
#endif

namespace TX_Randomizer
{
    public class HullNameDropdown : DropdownBase
    {
        public static event Action<HullBasicInfo.Name> OnHullNameDropdownChange = delegate { };

        private HullNameSlotAnimation slotAnimation;

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out slotAnimation);
        }

        private void Start()
        {
            dropdown.onValueChanged.AddListener(InvokeNameChangedEvent);    //Manual change and automatic
            if (slotAnimation)
            {
                //Apply first animated hulls, then the randomized combo
                slotAnimation.OnAnimationChanging += ApplyGivenHullName;
                slotAnimation.OnAnimationFinished += ApplyFinalHullName;
            }
            else
            {
                Randomizer.OnHullBasicInfoRandomized += ApplyGivenHullName;
            }
        }

        private void OnDestroy()
        {
            dropdown.onValueChanged.RemoveListener(InvokeNameChangedEvent);    //Manual change and automatic
            if (slotAnimation)
            {
                slotAnimation.OnAnimationChanging -= ApplyGivenHullName;
                slotAnimation.OnAnimationFinished -= ApplyFinalHullName;
            }
            else
            {
                Randomizer.OnHullBasicInfoRandomized -= ApplyGivenHullName;
            }
        }


        private void InvokeNameChangedEvent(int newHullNameIndex)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"HullNameDropdown: Invoking dropdown value changed event with {(HullBasicInfo.Name)newHullNameIndex}");
#endif
            OnHullNameDropdownChange?.Invoke((HullBasicInfo.Name)newHullNameIndex);
        }

        private void ApplyFinalHullName()
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"HullNameDropdown: ApplyFinalHullName({Randomizer.Instance.RandomizedCombo.Hull.BaseInfo.HullName}))");
#endif
            int index = -1;
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == Randomizer.Instance.RandomizedCombo.Hull.BaseInfo.HullName.ToString())
                {
#if DEBUG && UNITY_EDITOR
                    Debug.Log($"HullNameDropdown.ApplyFinalHullName(): Options[{i}] text({dropdown.options[i].text}) ==  {Randomizer.Instance.RandomizedCombo.Hull.BaseInfo.HullName} == ");
#endif
                    index = i;
                    break;
                }
            }
            if (index < 0)
            {
#if DEBUG && UNITY_EDITOR
                Debug.LogError($"Final Hull name not found");
#endif
                dropdown.value = 0;
            }
            else
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log($"HullNameDropdown.ApplyFinalHullName(): Final Index {index}");
#endif
                dropdown.value = index;
            }
        }

        private void ApplyGivenHullName(HullBasicInfo newHullBasicInfo)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"HullNameDropdown: ApplyGivenHullName({newHullBasicInfo.HullName}({(int)newHullBasicInfo.HullName}))");
#endif
            if (dropdown.value != (int)newHullBasicInfo.HullName)
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log($"HullNameDropdown.ApplyGivenHullName(): dropdown.value({dropdown.value}) NOT equal to (int)newHullBasicInfo.HullName ({newHullBasicInfo.HullName}({(int)newHullBasicInfo.HullName}))");
#endif
                dropdown.value = (int)newHullBasicInfo.HullName;
            }
#if DEBUG && UNITY_EDITOR
            else
            {
                Debug.Log($"HullNameDropdown.ApplyGivenHullName(): dropdown.value({dropdown.value}) equal to (int)newHullBasicInfo.HullName ({newHullBasicInfo.HullName}({(int)newHullBasicInfo.HullName}))");
            }
#endif
        }

        private void ApplyGivenHullName(HullBasicInfo newHullBasicInfo, List<HullInfo> _)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"HullNameDropdown: ApplyGivenHullName({newHullBasicInfo.HullName}({(int)newHullBasicInfo.HullName}))");
#endif
            if (dropdown.value != (int)newHullBasicInfo.HullName)
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log($"HullNameDropdown.ApplyGivenHullName(): dropdown.value({dropdown.value}) NOT equal to (int)newHullBasicInfo.HullName ({newHullBasicInfo.HullName}({(int)newHullBasicInfo.HullName}))");
#endif
                dropdown.value = (int)newHullBasicInfo.HullName;
            }
#if DEBUG && UNITY_EDITOR
            else
            {
                Debug.Log($"HullNameDropdown.ApplyGivenHullName(): dropdown.value({dropdown.value}) equal to (int)newHullBasicInfo.HullName ({newHullBasicInfo.HullName}({(int)newHullBasicInfo.HullName}))");
            }
#endif
        }
    }
}