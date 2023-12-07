using System.Collections.Generic;
using UnityEngine;

namespace TX_Randomizer
{
    [CreateAssetMenu(fileName ="New Turret Info", menuName ="Tank Component (for Wiki)/Turret Info")]
    [System.Serializable]
    public class TurretInfo : ScriptableObject
    {
        [SerializeField] private TurretBasicInfo baseInfo;
        [SerializeField] private List<TurretSkin> skins;
        [SerializeField] private AmmunitionListContainer ammunitions;

        public int NameIndex => (int)BaseInfo.TurretName - 1;

        public TurretBasicInfo BaseInfo { get => baseInfo; set => baseInfo = value; }
        public List<TurretSkin> Skins { get => skins; set => skins = value; }
        public List<Ammunition> Ammunitions => ammunitions.AmmunitionsList;
    }
}