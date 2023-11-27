using System;

namespace TX_Randomizer
{
    [Serializable]
    public static partial class BattleInfo
    {
        public static event Action OnBattleModeChanged = delegate { };
        public static event Action OnMapChanged = delegate { };
        public static event Action OnBattleTimeChanged = delegate { };
        public static event Action OnMaxTeamsChanged = delegate { };
        public static event Action OnGravityChanged = delegate { };
        public static event Action OnFriendlyFireChanged = delegate { };
        public static event Action OnKillzoneChanged = delegate { };
        public static event Action OnModulesAndGB_Changed = delegate { };

        private static GameMode _battleMode;
        private static Map _map;
        private static DurationTime _battleTime;
        private static int _maxPlayers;
        private static bool _friendlyFire;
        private static GravityName _gravity;
        private static bool _killzone;
        private static bool _modulesAndGB;

        public static GameMode BattleMode
        {
            get => _battleMode;
            set
            {
                _battleMode = value;
                OnBattleModeChanged?.Invoke();
            }
        }

        public static Map Map
        {
            get => _map;
            set
            {
                _map = value;
                OnMapChanged?.Invoke();
            }
        }

        public static DurationTime BattleTime
        {
            get => _battleTime;
            set
            {
                _battleTime = value;
                OnBattleTimeChanged?.Invoke();
            }
        }

        public static int MaxPlayers
        {
            get => _maxPlayers;
            set
            {
                _maxPlayers = value;
                OnMaxTeamsChanged?.Invoke();
            }
        }

        public static bool FriendlyFire
        {
            get => _friendlyFire;
            set
            {
                _friendlyFire = value;
                OnFriendlyFireChanged?.Invoke();
            }
        }

        public static GravityName Gravity
        {
            get => _gravity;
            set
            {
                _gravity = value;
                OnGravityChanged?.Invoke();
            }
        }

        public static bool Killzone
        {
            get => _killzone;
            set
            {
                _killzone = value;
                OnKillzoneChanged?.Invoke();
            }
        }

        public static bool ModulesAndGB
        {
            get => _modulesAndGB;
            set
            {
                _modulesAndGB = value;
                OnModulesAndGB_Changed?.Invoke();
            }
        }
    }
}