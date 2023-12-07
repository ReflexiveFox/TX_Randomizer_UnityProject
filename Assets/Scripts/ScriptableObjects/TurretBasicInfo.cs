using UnityEngine;

namespace TX_Randomizer
{
    [System.Serializable]
    public partial class TurretBasicInfo
    {
        public Name TurretName = Name.None;
        public Sprite Icon;

        public TurretBasicInfo(Name newTurretName)
        {
            TurretName = newTurretName;
            Icon = TankWiki.Instance.GetTurretInfo(newTurretName).BaseInfo.Icon;
        }
    }
}