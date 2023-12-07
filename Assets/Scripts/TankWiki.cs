#define DEBUG
#undef DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TX_Randomizer
{
    public class TankWiki : MonoBehaviour
    {
        public static event Action OnWikiReady = delegate { };

        public static TankWiki Instance;

        public const int MAX_PLAYERS = 10;
        [Header("String Dictionaries")]
        [SerializeField] private AmmunitionNameDictionary _ammunitionNameDictionary;
        [SerializeField] private BattleModeDictionary _battleModeDictionary;
        [SerializeField] private BattleTimeDictionary _battleTimeDictionary;
        [SerializeField] private CoatingNameDictionary _coatingNameDictionary;
        [SerializeField] private GravityNameDictionary _gravityNameDictionary;
        [SerializeField] private MapNameDictionary _mapNameDictionary;
        [SerializeField] private PaintNameDictionary _paintNameDictionary;
        [SerializeField] private SkinNameDictionary _skinNameDictionary;
        
        [Space]
        [SerializeField] private List<Map> _maps;

        [Header("Player equipment")]
        [SerializeField] private List<TurretInfo> _turrets;
        [SerializeField] private List<HullInfo> _hulls;

        [Header("Modules")]
        [SerializeField] private List<Module> _activeTurretModules;
        [SerializeField] private List<Module> _passiveTurretModules;

        [SerializeField] private List<Module> _activeHullModules;
        [SerializeField] private List<Module> _passiveHullModules;

        public List<Module> ActiveTurretModules => _activeTurretModules; 
        public List<Module> PassiveTurretModules => _passiveTurretModules;
        public List<Module> ActiveHullModules => _activeHullModules;
        public List<Module> PassiveHullModules => _passiveHullModules;

        [Space]
        [SerializeField] private List<Coating> _coatings;
        [SerializeField] private List<Paint> _paints;

        [SerializeField] private List<AmmunitionListContainer> _ammunitions;

        public List<TurretInfo> Turrets => _turrets;
        public List<HullInfo> Hulls => _hulls;

        public List<Coating> Coatings => _coatings;
        public List<Paint> Paints => _paints;
        public List<AmmunitionListContainer> Ammunitions => _ammunitions;

        public MapNameDictionary MapNamesDictionary => _mapNameDictionary;

        public List<Map> Maps => _maps;

        public BattleModeDictionary BattleModeDictionary => _battleModeDictionary; 
        public BattleTimeDictionary BattleTimeDictionary => _battleTimeDictionary;
        public CoatingNameDictionary CoatingNameDictionary => _coatingNameDictionary;
        public GravityNameDictionary GravityNameDictionary => _gravityNameDictionary;
        public PaintNameDictionary PaintNameDictionary => _paintNameDictionary;
        public SkinNameDictionary SkinNameDictionary => _skinNameDictionary;

        private void Awake()
        {
            Instance = this;   
        }

        public List<TurretSkin> GetTurretSkinsList(TurretBasicInfo.Name turretName)
        {
            if(turretName is TurretBasicInfo.Name.None)
            {
                return null;
            }

            try
            {
                return new List<TurretSkin>(_turrets[(int)turretName].Skins);
            }
            catch(Exception)
            {
#if DEBUG && UNITY_EDITOR
                Debug.LogError($"GetTurretSkinsList({turretName}) with index: {(int)turretName}");
#endif
                return null;
            }
        }

        public TurretSkin GetTurretSkin(TurretBasicInfo.Name turretName, int skinIndex)
        {
            return _turrets[(int)turretName].Skins[skinIndex];
        }

        public List<HullSkin> GetHullSkinsList(HullBasicInfo.Name hullName)
        {
            if (hullName is HullBasicInfo.Name.None)
            {
                return null;
            }

            try
            {
                return new List<HullSkin>(_hulls[(int)hullName].Skins);
            }
            catch (Exception)
            {
#if DEBUG && UNITY_EDITOR
                Debug.LogError($"GetTurretSkinsList({hullName}) with index: {(int)hullName}");
#endif
                return null;
            }
        }

        public HullSkin GetHullSkin(HullBasicInfo.Name hullName, int skinIndex)
        {
            return _hulls[(int)hullName].Skins[skinIndex];
        }

        public Module GetModule(ModuleType moduleType, int moduleIndex)
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"TankWiki: Getting module of type {moduleType.ActiveType}");
#endif
            try
            {
                if (moduleType.TecnologyType is TankPartType.Hull)
                {
                    if (moduleType.ActiveType is ModuleType.ActivationType.Passive)
                    {
                        return _passiveHullModules[moduleIndex];
                    }
                    else if (moduleType.ActiveType is ModuleType.ActivationType.Active)
                    {
                        return _activeHullModules[moduleIndex];
                    }
                    else
                    {
#if DEBUG && UNITY_EDITOR
                        Debug.LogError("ActivationType is not defined");
#endif
                        return null;
                    }
                }
                else if (moduleType.TecnologyType is TankPartType.Turret)
                {
                    if (moduleType.ActiveType is ModuleType.ActivationType.Passive)
                    {

                        return _passiveTurretModules[moduleIndex];
                    }
                    else if (moduleType.ActiveType is ModuleType.ActivationType.Active)
                    {
                        return _activeTurretModules[moduleIndex];
                    }
                    else
                    {
#if DEBUG && UNITY_EDITOR
                        Debug.LogError("ActivationType is not defined");
#endif
                        return null;
                    }
                }
                else
                {
#if DEBUG && UNITY_EDITOR
                    Debug.LogError("TecnologyType is not defined");
#endif
                    return null;
                }
            }
            catch(ArgumentOutOfRangeException e)
            {
#if DEBUG && UNITY_EDITOR
                Debug.LogError($"Errore di indice! {e.ParamName} ");
#endif
                return null;
            }

        }
   
        public List<Ammunition> GetAmmunitionListOf(TurretBasicInfo.Name turretOwnerName)
        {
            foreach(AmmunitionListContainer ammoList in Ammunitions)
            {
                if(ammoList.TurretOwned == turretOwnerName)
                    return new List<Ammunition>(ammoList.AmmunitionsList);
            }
#if DEBUG && UNITY_EDITOR
            Debug.LogError($"Ammunition list of {turretOwnerName} not found");
#endif
            return null;
        }

        public AmmunitionListContainer GetAmmunitionListContainer(TurretBasicInfo.Name turretOwnerName)
        {
            foreach (AmmunitionListContainer ammoList in Ammunitions)
            {
                if (ammoList.TurretOwned == turretOwnerName)
                    return ammoList;
            }
#if DEBUG && UNITY_EDITOR
            Debug.LogError($"Ammunition list container of {turretOwnerName} not found");
#endif
            return null;
        }

        public Ammunition GetAmmunition(Ammunition.Name ammoName, TurretBasicInfo.Name turretOwnerName)
        {
            foreach(AmmunitionListContainer ammoListContainer in Ammunitions)
            {
                if(ammoListContainer.TurretOwned == turretOwnerName)
                {
#if DEBUG && UNITY_EDITOR
                    Debug.Log($"AmmoListContainer of {ammoListContainer.TurretOwned} == Parameter {turretOwnerName}");
#endif
                    foreach(Ammunition ammunition in ammoListContainer.AmmunitionsList)
                    {
#if DEBUG && UNITY_EDITOR
                        Debug.Log($"Ammunition to get: {ammoName}, Iterative Ammunition: {ammunition.AmmoName}");
#endif
                        if (ammunition.AmmoName == ammoName)
                        {
                            return ammunition;
                        }
                    }
#if DEBUG && UNITY_EDITOR
                    Debug.LogError($"Ammunition \"{ammoName}\" of {turretOwnerName} not found");
#endif
                    return null;
                }

            }
#if DEBUG && UNITY_EDITOR
            Debug.LogError($"AmmunitionListContainer of {turretOwnerName} not found");
#endif
            return null;
        }

        public TurretInfo GetTurretInfo(TurretBasicInfo.Name turretName)
        {
            foreach(TurretInfo turretInfo in Turrets)
            {
                if(turretInfo.BaseInfo.TurretName == turretName)
                {
                    return turretInfo;
                }
            }
#if DEBUG && UNITY_EDITOR
            Debug.LogWarning($"There is no turret called {turretName}, returning None");
#endif
            return GetNoneInfoOf(Turrets);
        }

        public HullInfo GetHullInfo(HullBasicInfo.Name hullName)
        {
            foreach (HullInfo hullInfo in Hulls)
            {
                if (hullInfo.BaseInfo.HullName == hullName)
                {
                    return hullInfo;
                }
            }
#if DEBUG && UNITY_EDITOR
            Debug.LogWarning($"There is no hull called {hullName}, returning None");
#endif
            return GetNoneInfoOf(Hulls);
        }

        /// <summary>
        /// Returns the first element of a list to obtain the "None" info
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetNoneInfoOf<T>(List<T> list) where T : ScriptableObject
        {
            return list[0];    //Return the last element for none info
        }
    }
}