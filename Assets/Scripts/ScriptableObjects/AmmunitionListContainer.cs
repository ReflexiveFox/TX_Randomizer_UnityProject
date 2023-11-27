using UnityEngine;
using System.Collections.Generic;

namespace TX_Randomizer
{
    [CreateAssetMenu(fileName = "New Ammunition List", menuName = "Ammunition list")]
    public class AmmunitionListContainer : ScriptableObject
    {
        public TurretBasicInfo.Name TurretOwned;
        [SerializeField] private List<Ammunition> _ammunitions;

        public List<Ammunition> AmmunitionsList => _ammunitions;
    }
}