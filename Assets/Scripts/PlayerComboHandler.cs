#define DEBUG
#undef DEBUG

using System;
using UnityEngine;

namespace TX_Randomizer
{
    [System.Serializable]
    public partial class PlayerComboHandler : MonoBehaviour
    {
        public PlayerCombo PlayerCombo;
        public static PlayerComboHandler Instance = null;


        private void Awake()
        {
            Instance = this;
            TankWiki.OnWikiReady += InitializeCombo;
        }

        private void Start()
        {
            TurretNameDropdown.OnTurretNameDropdownChange += ChangeTurret;
            HullNameDropdown.OnHullNameDropdownChange += ChangeHull;
            AmmunitionNameDropdown.OnDropdownValueChanged += ChangeAmmunition;
        }

        private void OnDestroy()
        {
            TurretNameDropdown.OnTurretNameDropdownChange -= ChangeTurret;
            HullNameDropdown.OnHullNameDropdownChange -= ChangeHull;
            AmmunitionNameDropdown.OnDropdownValueChanged -= ChangeAmmunition;
            TankWiki.OnWikiReady -= InitializeCombo;
        }

        private void InitializeCombo()
        {
            PlayerCombo.InitializeCombo();
        }

        private void ChangeAmmunition(TurretBasicInfo.Name turretOwnedName, int ammoIndex)
        {
            if (turretOwnedName == PlayerCombo.Turret.BaseInfo.TurretName)
            {
                PlayerCombo.Turret.Ammunition = TankWiki.Instance.GetAmmunitionListOf(turretOwnedName)[ammoIndex];
            }
            else
            {
#if DEBUG && UNITY_EDITOR
                Debug.LogWarning($"Parameter ({turretOwnedName}) is different from saved turret name ({PlayerCombo.Turret.BaseInfo.TurretName}), setting to None");
#endif
                PlayerCombo.Turret.Ammunition = TankWiki.GetNoneInfoOf(TankWiki.Instance.GetAmmunitionListOf(TurretBasicInfo.Name.None));
            }
        }

        private void ChangeHull(HullBasicInfo.Name newHullName)
        {
            PlayerCombo.Hull.BaseInfo = TankWiki.Instance.GetHullInfo(newHullName).BaseInfo;
        }        

        private void ChangeTurret(TurretBasicInfo.Name newTurretName)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"PlayerCombo: ChangeTurret({newTurretName})");
#endif
            PlayerCombo.Turret.BaseInfo = TankWiki.Instance.GetTurretInfo(newTurretName).BaseInfo;
            PlayerCombo.Turret.Ammunition = TankWiki.Instance.GetAmmunitionListOf(newTurretName)[0];
            PlayerCombo.Coating = TankWiki.GetNoneInfoOf(TankWiki.Instance.Coatings);
        }
    }
}