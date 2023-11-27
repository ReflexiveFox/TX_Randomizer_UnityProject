using System.Collections.Generic;
using UnityEngine;

namespace TX_Randomizer
{
    [CreateAssetMenu(fileName = "New Hull Info", menuName = "Tank Component (for Wiki)/Hull Info")]
    [System.Serializable]
    public class HullInfo : ScriptableObject
    {
        public HullBasicInfo BaseInfo;
        public List<HullSkin> Skins;

        public int NameIndex => (int)BaseInfo.HullName - 1;
    }
}