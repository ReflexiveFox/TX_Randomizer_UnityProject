using UnityEngine;

namespace TX_Randomizer
{
    [System.Serializable]
    public partial class HullBasicInfo
    {
        public Name HullName = Name.None;
        public Sprite Icon;

        public HullBasicInfo(Name newHullName)
        {
            HullName = newHullName;
            Icon = TankWiki.Instance.GetHullInfo(newHullName).BaseInfo.Icon;
        }
    }
}