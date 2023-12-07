#define DEBUG
#undef DEBUG

using System;
using UnityEngine;

namespace TX_Randomizer
{
    [System.Serializable]
    public class PlayerCombo
    {
        public event Action<Coating> OnCoatingChanged = delegate { };
        public event Action<Paint> OnPaintChanged = delegate { };

        [SerializeField] private Turret _turret;
        [SerializeField] private Coating _coating;
        [SerializeField] private Hull _hull;
        [SerializeField] private Paint _paint;
        [SerializeField] private ModulesArray _modules;

        public Turret Turret
        {
            get
            {
                if (_turret is null)
                {
                    _turret.DefaultTurret();
                }
                return _turret;
            }
        }
        
        public Coating Coating
        {
            get => _coating is null ? TankWiki.GetNoneInfoOf(TankWiki.Instance.Coatings) : _coating;
            set
            {
                _coating = value;
                OnCoatingChanged?.Invoke(_coating);
            }
        }

        public Hull Hull
        {
            get
            {
                if (_hull is null)
                {
                    _hull.DefaultHull();
                }
                return _hull;
            }
        }

        public Paint Paint
        {
            get => _paint is null ? TankWiki.GetNoneInfoOf(TankWiki.Instance.Paints) : _paint;
            set
            {
                _paint = value;
                OnPaintChanged?.Invoke(_paint);
            }
        }

        public ModulesArray ModulesArray => _modules;


        public void InitializeCombo()
        {
            _turret.DefaultTurret();
            _coating = TankWiki.GetNoneInfoOf(TankWiki.Instance.Coatings);
            _hull.DefaultHull();
            _paint = TankWiki.GetNoneInfoOf(TankWiki.Instance.Paints);
            
            _modules = new ();
        }
    }
}