#define DEBUG
#undef DEBUG

using System;
using UnityEngine;

namespace TX_Randomizer
{
    [System.Serializable]
    public class Hull
    {
        public event Action<HullBasicInfo> OnHullBasicInfoChanged = delegate { };
        public event Action<HullSkin> OnHullSkinChanged = delegate { };

        [SerializeField] private HullBasicInfo _baseInfo;
        [SerializeField] private HullSkin _skin;

        public HullBasicInfo BaseInfo
        {
            get => _baseInfo;
            set
            {
                _baseInfo = value;
#if DEBUG && UNITY_EDITOR
                Debug.Log($"Hull: OnHullBasicInfoChanged => {_baseInfo.HullName}");
#endif
                OnHullBasicInfoChanged?.Invoke(_baseInfo);
            }
        }

        public HullSkin Skin
        {
            get => _skin;
            set
            {
                _skin = value;
                OnHullSkinChanged?.Invoke(_skin);
            }
        }

        public void DefaultHull()
        {
            BaseInfo = TankWiki.GetNoneInfoOf(TankWiki.Instance.Hulls).BaseInfo;
            Skin = null;
        }

        public void DefaultHull(HullBasicInfo.Name newHullName)
        {
            BaseInfo = new(newHullName);
        }
    }
}