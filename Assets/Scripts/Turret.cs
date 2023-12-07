#define DEBUG
#undef DEBUG

using System;
using UnityEngine;

namespace TX_Randomizer
{
    [Serializable]
    public class Turret 
    {
        public event Action<TurretBasicInfo> OnTurretBasicInfoChanged = delegate { };
        public event Action<TurretSkin> OnTurretSkinChanged = delegate { };
        public event Action<Ammunition> OnAmmunitionChanged = delegate { };

        [SerializeField] private TurretBasicInfo _baseInfo;
        [SerializeField] private TurretSkin _skin;
        [SerializeField] private Ammunition _ammunition;

        public TurretBasicInfo BaseInfo
        {
            get => _baseInfo;
            set
            {
                _baseInfo = value;
#if DEBUG && UNITY_EDITOR
                Debug.Log($"Turret: OnTurretBasicInfoChanged => {_baseInfo.TurretName}");
#endif
                OnTurretBasicInfoChanged?.Invoke(_baseInfo);
            }
        }

        public TurretSkin Skin
        {
            get => _skin;
            set
            {
                _skin = value;
                OnTurretSkinChanged?.Invoke(_skin);
            }
        }

        public Ammunition Ammunition
        {
            get => _ammunition;
            set
            {
                _ammunition = value;
                OnAmmunitionChanged?.Invoke(_ammunition);
            }
        }

        public void DefaultTurret()
        {
            BaseInfo = TankWiki.GetNoneInfoOf(TankWiki.Instance.Turrets).BaseInfo;
            Skin = null;
            Ammunition = TankWiki.GetNoneInfoOf(TankWiki.Instance.GetAmmunitionListOf(TurretBasicInfo.Name.None));
        }

        public Turret(TurretBasicInfo.Name newTurretName)
        {
            BaseInfo = new(newTurretName);
            AmmunitionListContainer ammoContainer = TankWiki.GetNoneInfoOf(TankWiki.Instance.Ammunitions);
            Ammunition = TankWiki.GetNoneInfoOf(ammoContainer.AmmunitionsList);
        }
    }
}