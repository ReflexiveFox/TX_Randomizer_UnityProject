using System;

namespace TX_Randomizer
{
    public class HullNameDropdown : DropdownBase
    {
        private void Start()
        {
            Randomizer.OnHullBasicInfoRandomized += ApplyHullName;
        }


        private void OnDestroy()
        {
            Randomizer.OnHullBasicInfoRandomized -= ApplyHullName;
        }

        private void ApplyHullName(HullBasicInfo newHullBasicInfo)
        {
            dropdown.value = (int)newHullBasicInfo.HullName;
        }
    }
}