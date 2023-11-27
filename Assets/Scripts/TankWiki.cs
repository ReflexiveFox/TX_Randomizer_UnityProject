using System;
using System.Collections.Generic;
using UnityEngine;

namespace TX_Randomizer
{
    public class TankWiki : MonoBehaviour
    {
        public static TankWiki instance;
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
            if (instance != null && instance != this)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        public List<TurretSkin> GetTurretSkinsList(TurretBasicInfo.Name turretName)
        {
            int index = (int)turretName -1;
            return new List<TurretSkin>(_turrets[index].Skins);
        }

        public List<HullSkin> GetHullSkinsList(HullBasicInfo.Name hullName)
        {
            int index = (int)hullName - 1;
            return new List<HullSkin>(_hulls[index].Skins);
        }

        public Module GetModule(ModuleType moduleType, int moduleIndex)
        {
            Debug.Log($"TankWiki: Getting module of type {moduleType.ActiveType}");
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
                        Debug.LogError("ActivationType is not defined");
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
                        Debug.LogError("ActivationType is not defined");
                        return null;
                    }
                }
                else
                {
                    Debug.LogError("TecnologyType is not defined");
                    return null;
                }
            }catch(ArgumentOutOfRangeException e)
            {
                Debug.LogError($"Errore di indice! {e.ParamName} ");
                return null;
            }

        }
   
        public List<Ammunition> GetAmmunitionList(TurretBasicInfo.Name turretName)
        {
            foreach(AmmunitionListContainer ammoList in Ammunitions)
            {
                if(ammoList.TurretOwned == turretName)
                    return new List<Ammunition>(ammoList.AmmunitionsList);
            }
            Debug.LogError($"Ammunition list of {turretName} not found");
            return null;
        }

        public AmmunitionListContainer GetAmmunitionListContainer(TurretBasicInfo.Name turretName)
        {
            foreach (AmmunitionListContainer ammoList in Ammunitions)
            {
                if (ammoList.TurretOwned == turretName)
                    return ammoList;
            }
            Debug.LogError($"Ammunition list container of {turretName} not found");
            return null;
        }

        public Ammunition GetAmmunition(string ammoName, TurretBasicInfo.Name turretName)
        {
            foreach(AmmunitionListContainer ammoListContainer in Ammunitions)
            {
                if(ammoListContainer.TurretOwned == turretName)
                {
                    foreach(Ammunition ammunition in ammoListContainer.AmmunitionsList)
                    {
                        if (ammunition.AmmoName.ToString() == ammoName)
                        {
                            return ammunition;
                        }
                    }
                    Debug.LogError($"Ammunition \"{ammoName}\" of {turretName} not found");
                    return null;
                }
                
            }
            Debug.LogError($"AmmunitionListContainer of {turretName} not found");
            return null;
        }
    }
}