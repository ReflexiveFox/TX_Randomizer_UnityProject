using UnityEngine;

namespace TX_Randomizer
{
    [System.Serializable]
    public partial class HullBasicInfo
    {
        public Name HullName = Name.None;
        public Sprite Icon;

        public HullBasicInfo()
        {
            HullName = Name.None;
            Icon = null;
        }
    }
}