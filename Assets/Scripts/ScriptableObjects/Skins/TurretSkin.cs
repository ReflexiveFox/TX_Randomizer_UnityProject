using UnityEngine;

namespace TX_Randomizer
{
    [CreateAssetMenu(fileName = "New Turret Skin", menuName = "Skin/Turret Skin")]
    public class TurretSkin : Skin
    {
        [SerializeField] private TurretBasicInfo.Name _turretOwned;

        public TurretBasicInfo.Name TurretOwned => _turretOwned;
    }
}