using UnityEngine;

namespace TX_Randomizer
{
    [System.Serializable]
    public partial class Ammunition : ScriptableObject
    {
        public Sprite Sprite;
        public Name AmmoName;
        public TurretBasicInfo.Name TurretOwned;

        public void DefaultConstructor()
        {
            Sprite = null;
            AmmoName = Name.None;
            TurretOwned = TurretBasicInfo.Name.None;
        }
    }
}