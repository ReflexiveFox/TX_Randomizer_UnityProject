using System.Collections.Generic;

namespace TX_Randomizer
{
    [System.Serializable]
    public class Turret
    {
        public event System.Action OnCoatingChanged = delegate { };

        public TurretBasicInfo BaseInfo;
        public TurretSkin Skin;
        private Coating _coating;
        public List<Ammunition> Ammunitions;

        public int NameIndex => (int)BaseInfo.TurretName - 1;

        public Coating Coating
        {
            get => _coating;
            set
            {
                _coating = value;
                OnCoatingChanged?.Invoke();
            }
        }

        public Turret()
        {
            BaseInfo = new();
            Coating = TankWiki.instance.Coatings[0];
            Ammunitions = new List<Ammunition>();
        }
    }
}