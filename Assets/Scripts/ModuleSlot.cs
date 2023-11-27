using UnityEngine;
using UnityEngine.UI;

namespace TX_Randomizer
{
    public class ModuleSlot : MonoBehaviour
    {
        [SerializeField] private ModuleType _moduleSlotType;
        [SerializeField] private int ID;

        [Header("References")]
        [SerializeField] private Image _moduleImage;

        private void Start()
        {
            Randomizer.OnModuleSlotRandomized += ApplyModule;
        }

        private void OnDestroy()
        {
            Randomizer.OnModuleSlotRandomized -= ApplyModule;
        }

        public void ValidateCombo()
        {
            for(int i = 0; i < 6; i++)
            {
                ApplyModule(PlayerCombo.ModulesArray.Modules[i], i);
            }
        }

        private void ApplyModule(Module newModule, int positionIndex)
        {
            if (_moduleSlotType != newModule.Type)
            {
                return;
            }
            else
            {
                if (positionIndex == ID)
                {
                    _moduleImage.sprite = newModule.Icon;
                }
            }
        }
    }
}