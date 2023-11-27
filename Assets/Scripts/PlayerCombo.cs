using UnityEngine;

namespace TX_Randomizer
{
    [System.Serializable]
    public static class PlayerCombo
    {
        [SerializeField] private static Turret _turret;
        [SerializeField] private static Hull _hull;
        [SerializeField] private static ModulesArray _modules;
        public static Ammunition Ammunition;

        public static Turret Turret => _turret;
        public static Hull Hull => _hull;
        public static ModulesArray ModulesArray => _modules;

        static PlayerCombo()
        {
            _turret = new();
            _hull = new();
            _modules = new ModulesArray();
        }
    }
}