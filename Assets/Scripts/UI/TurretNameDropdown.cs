using System;

namespace TX_Randomizer
{
    public class TurretNameDropdown : DropdownBase
    {
        private void Start()
        {
            Randomizer.OnTurretBasicInfoRandomized += ApplyTurretName;
        }


        private void OnDestroy()
        {
            Randomizer.OnTurretBasicInfoRandomized -= ApplyTurretName;
        }

        private void ApplyTurretName(TurretBasicInfo newTurretBasicInfo)
        {
            dropdown.value = (int)newTurretBasicInfo.TurretName;
        }
    }
}