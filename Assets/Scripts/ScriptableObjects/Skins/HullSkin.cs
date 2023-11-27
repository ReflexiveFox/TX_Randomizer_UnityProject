using UnityEngine;

namespace TX_Randomizer
{
    [CreateAssetMenu(fileName = "New Hull Skin", menuName = "Skin/Hull Skin")]
    public class HullSkin : Skin
    {
        [SerializeField] private HullBasicInfo.Name _hullOwned;

        public HullBasicInfo.Name TurretOwned => _hullOwned;
    }
}