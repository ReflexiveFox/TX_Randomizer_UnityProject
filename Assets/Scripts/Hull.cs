namespace TX_Randomizer
{
    public class Hull
    {
        public event System.Action OnPaintChanged = delegate { };

        public HullBasicInfo BaseInfo;
        public HullSkin Skin;
        private Paint _paint;

        public int NameIndex => (int)BaseInfo.HullName - 1;

        public Paint Paint
        {
            get => _paint;
            set
            {
                _paint = value;
                OnPaintChanged?.Invoke();
            }
        }

        public Hull()
        {
            _paint = TankWiki.instance.Paints[0];
        }
    }
}