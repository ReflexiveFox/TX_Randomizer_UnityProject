using System.Collections.Generic;
using UnityEngine;

namespace TX_Randomizer
{
    [CreateAssetMenu(fileName ="New Turret Info", menuName ="Tank Component (for Wiki)/Turret Info")]
    [System.Serializable]
    public class TurretInfo : ScriptableObject
    {
        public TurretBasicInfo BaseInfo;
        public List<TurretSkin> Skins;
        public List<Ammunition> Ammunitions;

        public int NameIndex => (int)BaseInfo.TurretName - 1;
    }
}