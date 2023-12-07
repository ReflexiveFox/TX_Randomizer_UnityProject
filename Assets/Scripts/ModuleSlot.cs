using System;
using UnityEngine;
using UnityEngine.UI;

namespace TX_Randomizer
{
    public class ModuleSlot : MonoBehaviour
    {
        public event Action<Module> CanApplyModule = delegate { };

        [SerializeField] private ModuleType _moduleSlotType;
        [SerializeField] private int ID;

        [Header("References")]
        [SerializeField] private Image moduleImage;
        private ModuleSlotAnimation slotAnimation;
        private Module currentModule = null;

        private void Awake()
        {
            slotAnimation = GetComponentInChildren<ModuleSlotAnimation>();
        }

        private void Start()
        {
            Randomizer.OnModuleSlotRandomized += SaveModule;
            if(slotAnimation)
            {
                slotAnimation.OnAnimationFinished += ApplyModule;
            }
        }

        private void OnDestroy()
        {
            Randomizer.OnModuleSlotRandomized -= SaveModule;
            if (slotAnimation)
            {
                slotAnimation.OnAnimationFinished -= ApplyModule;
            }
        }

        public void ValidateCombo()
        {
            for(int i = 0; i < 6; i++)
            {
                SaveModule(PlayerComboHandler.Instance.PlayerCombo.ModulesArray.Modules[i], i);
            }
        }

        private void SaveModule(Module newModule, int positionIndex)
        {
            if (_moduleSlotType != newModule.Type)
            {
                return;
            }
            else
            {
                if (positionIndex == ID)
                {
                    currentModule = newModule;
                    if(slotAnimation)
                    {
                        slotAnimation.PlaySlotAnimation(newModule);
                    }
                }
            }
        }

        private void ApplyModule()
        {
            moduleImage.sprite = currentModule.Icon;
        }
    }
}