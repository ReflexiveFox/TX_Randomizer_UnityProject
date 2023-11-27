using UnityEngine;

namespace TX_Randomizer
{
    [System.Serializable]
    public partial class TurretBasicInfo
    {
        public Name TurretName = Name.None;
        public Sprite Icon;

        public TurretBasicInfo()
        {
            TurretName = Name.None;
            Icon = null;
        }
    }
}