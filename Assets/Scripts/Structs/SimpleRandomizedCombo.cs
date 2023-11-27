namespace TX_Randomizer
{
    [System.Serializable]
    public struct SimpleRandomizedCombo
    {
        public int turretChosen;
        public int hullChosen;
        public Module[] modulesChosen;

        public SimpleRandomizedCombo(int turretChosen = 0, int hullChosen = 0) : this()
        {
            this.turretChosen = turretChosen;
            this.hullChosen = hullChosen;
            this.modulesChosen = new Module[6];
        }
    }
}