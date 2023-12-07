#define DEBUG
#undef DEBUG

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TX_Randomizer
{
    public class Randomizer : MonoBehaviour
    {
        public static event Action<TurretBasicInfo, List<TurretInfo>> OnTurretBasicInfoRandomized = delegate { };
        public static event Action<HullBasicInfo, List<HullInfo>> OnHullBasicInfoRandomized = delegate { };

        public static event Action<TurretSkin> OnTurretSkinRandomized = delegate { };
        public static event Action<HullSkin> OnHullSkinRandomized = delegate { };

        public static event Action<Module, int> OnModuleSlotRandomized = delegate { };

        public static event Action<AmmunitionListContainer, Ammunition> OnAmmunitionSlotRandomized = delegate { };
        public static event Action<Coating> OnCoatingSlotRandomized = delegate { };

        [SerializeField] private PlayerCombo _randomizedCombo;

        public static Randomizer Instance;

        public PlayerCombo RandomizedCombo 
        { 
            get => _randomizedCombo; 
            private set => _randomizedCombo = value;
        }

        private void Awake()
        {
            Instance = this;
        }

        public void RandomizeMatchParameters()
        {
            RandomizeMap();
            RandomizeGameMode();
            RandomizeBattleTime();
            RandomizeMaxTeams();
            RandomizeGravity();
            RandomizeFriendlyFire();
            RandomizeKillzone();
            RandomizeModulesAndGB();
        }

        public void RandomizeModulesAndGB() => BattleInfo.ModulesAndGB = RandomizeBool();

        public void RandomizeKillzone() => BattleInfo.Killzone = RandomizeBool();

        public void RandomizeFriendlyFire() => BattleInfo.FriendlyFire = RandomizeBool();

        public void RandomizeGravity() => BattleInfo.Gravity = RandomizeElementInEnum(BattleInfo.Gravity);

        public void RandomizeMaxTeams()
        {
            int currentOption = BattleInfo.MaxPlayers;
            List<int> intList = new();
            for(int i = 1; i <= TankWiki.MAX_PLAYERS; i++)
            {
                intList.Add(i);
            }

            if(intList.Contains(currentOption))
            {
                intList.Remove(currentOption);
            }

            BattleInfo.MaxPlayers = intList[Random.Range(0, intList.Count)];
        }

        public void RandomizeBattleTime() => BattleInfo.BattleTime = RandomizeElementInEnum(BattleInfo.BattleTime);

        public void RandomizeGameMode() => BattleInfo.BattleMode = RandomizeElementInEnum(BattleInfo.BattleMode);

        public void RandomizeMap()
        {
            Map currentMap = BattleInfo.Map;
            List<Map> mapsCopyList = new List<Map>(TankWiki.Instance.Maps);
            
            if(currentMap is null)
            {
                currentMap = mapsCopyList[0];
            }
            mapsCopyList.RemoveAt(0);

            int duplicateIndex = GetExistingMapItemIndex(mapsCopyList, currentMap.MapName);
            if(duplicateIndex > 0)
            {
                mapsCopyList.RemoveAt(duplicateIndex);
            }
            BattleInfo.Map = mapsCopyList[Random.Range(0, mapsCopyList.Count)];            
        }

        private int GetExistingMapItemIndex(List<Map> mapsCopyList, Map.Name mapName)
        {
            for (int i = 0; i < mapsCopyList.Count; i++)
            {
                if (mapsCopyList[i].MapName == mapName)
                {
                    return i;
                }
            }
            return -1;
        }

        public void RandomizePlayerCombo()
        {
            RandomizeTurretBaseInfo();
            RandomizeHullBaseInfo();
            
            RandomizeModules();
            
            RandomizeCoating();
            RandomizeAmmunition();
            RandomizePaint();
        }

        public void RandomizeCoating()
        {
            Coating currentCoating = PlayerComboHandler.Instance.PlayerCombo.Coating;
            List<Coating> coatingListCopy = new List<Coating>(TankWiki.Instance.Coatings);
            coatingListCopy.RemoveAt(0);
            int duplicateIndex = GetExistingCoatingItemIndex(coatingListCopy, currentCoating.CoatingName);
            if(duplicateIndex >= 0)
            {
                coatingListCopy.RemoveAt(duplicateIndex);
            }
            RandomizedCombo.Coating = coatingListCopy[Random.Range(0, coatingListCopy.Count)];
            OnCoatingSlotRandomized?.Invoke(RandomizedCombo.Coating);
        }

        public void RandomizePaint()
        {
            Paint currentPaint = PlayerComboHandler.Instance.PlayerCombo.Paint;
            List<Paint> paintListCopy = new List<Paint>(TankWiki.Instance.Paints);
            paintListCopy.RemoveAt(0);
            int duplicateIndex = GetExistingPaintItemIndex(paintListCopy, currentPaint.PaintName);
            if (duplicateIndex >= 0)
            {
                paintListCopy.RemoveAt(duplicateIndex);
            }
            RandomizedCombo.Paint = paintListCopy[Random.Range(0, paintListCopy.Count)];
        }

        public void RandomizeTurretBaseInfo()
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"Randomizer.RandomizeTurretBaseInfo(): Before change {PlayerComboHandler.Instance.PlayerCombo.Turret.BaseInfo.TurretName}");
#endif
            TurretBasicInfo currentTurretBasicInfo = PlayerComboHandler.Instance.PlayerCombo.Turret.BaseInfo;

            if (currentTurretBasicInfo == null)
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log($"Randomizer.RandomizeTurretBaseInfo(): {currentTurretBasicInfo} is null");
#endif
                return;
            }

#if DEBUG && UNITY_EDITOR
            Debug.Log($"Randomizer.RandomizeTurretBaseInfo(): {currentTurretBasicInfo} is not null");
#endif
            List<TurretInfo> turretInfoCopy = CreateCleanTurretList(currentTurretBasicInfo);
            int randomIndex = Random.Range(0, turretInfoCopy.Count);
            RandomizedCombo.Turret.BaseInfo = turretInfoCopy[randomIndex].BaseInfo;

            OnTurretBasicInfoRandomized?.Invoke(RandomizedCombo.Turret.BaseInfo, turretInfoCopy);
#if DEBUG && UNITY_EDITOR
            Debug.Log($"Randomizer.RandomizeTurretBaseInfo(): After change {PlayerComboHandler.Instance.PlayerCombo.Turret.BaseInfo.TurretName}");
#endif
        }

        private List<TurretInfo> CreateCleanTurretList(TurretBasicInfo currentTurretBasicInfo)
        {
            List<TurretInfo> turretInfoCopy = new List<TurretInfo>(TankWiki.Instance.Turrets);
            //Remove None element
            if(currentTurretBasicInfo.TurretName != TurretBasicInfo.Name.None)
            {
                int duplicateIndex = -1, noneIndex = -1;
                for(int i = 0; i < turretInfoCopy.Count; i++)
                {
                    if(turretInfoCopy[i].BaseInfo.TurretName == currentTurretBasicInfo.TurretName)
                    {
                        duplicateIndex = i;
                        if (noneIndex != -1)
                        {
                            break;
                        }
                    }
                    else if(turretInfoCopy[i].BaseInfo.TurretName == TurretBasicInfo.Name.None)
                    {
                        noneIndex = i;
                        if (duplicateIndex != -1)
                        {
                            break;
                        }
                    }
                }
                if (duplicateIndex >= 0)
                {
                    turretInfoCopy.RemoveAt(duplicateIndex);
                }
                if(noneIndex >= 0)
                {
                    turretInfoCopy.RemoveAt(noneIndex);
                }
            }
            else
            {
                int noneIndex = -1;
                for (int i = 0; i < turretInfoCopy.Count; i++)
                {
                    if (turretInfoCopy[i].BaseInfo.TurretName == TurretBasicInfo.Name.None)
                    {
                        noneIndex = i;
                        break;
                    }
                }
                if (noneIndex >= 0)
                {
                    turretInfoCopy.RemoveAt(noneIndex);
                }
            }

            return turretInfoCopy;
        }

        private List<HullInfo> CreateCleanHullList(HullBasicInfo currentHullBasicInfo)
        {
            List<HullInfo> hullInfoCopy = new List<HullInfo>(TankWiki.Instance.Hulls);
            //Remove None element
            if (currentHullBasicInfo.HullName != HullBasicInfo.Name.None)
            {
                int duplicateIndex = -1, noneIndex = -1;
                for (int i = 0; i < hullInfoCopy.Count; i++)
                {
                    if (hullInfoCopy[i].BaseInfo.HullName == currentHullBasicInfo.HullName)
                    {
                        duplicateIndex = i;
                        if (noneIndex != -1)
                        {
                            break;
                        }
                    }
                    else if (hullInfoCopy[i].BaseInfo.HullName == HullBasicInfo.Name.None)
                    {
                        noneIndex = i;
                        if (duplicateIndex != -1)
                        {
                            break;
                        }
                    }
                }
                if (duplicateIndex >= 0)
                {
                    hullInfoCopy.RemoveAt(duplicateIndex);
                }
                if (noneIndex >= 0)
                {
                    hullInfoCopy.RemoveAt(noneIndex);
                }
            }
            else
            {
                int noneIndex = -1;
                for (int i = 0; i < hullInfoCopy.Count; i++)
                {
                    if (hullInfoCopy[i].BaseInfo.HullName == HullBasicInfo.Name.None)
                    {
                        noneIndex = i;
                        break;
                    }
                }
                if (noneIndex >= 0)
                {
                    hullInfoCopy.RemoveAt(noneIndex);
                }
            }

            return hullInfoCopy;
        }

        public void RandomizeHullBaseInfo()
        {
#if DEBUG && UNITY_EDITOR
            Debug.Log($"Randomizer.RandomizeHullBaseInfo(): Before change {PlayerComboHandler.Instance.PlayerCombo.Hull.BaseInfo.HullName}");
#endif
            HullBasicInfo currentHullBasicInfo = PlayerComboHandler.Instance.PlayerCombo.Hull.BaseInfo;

            if (currentHullBasicInfo == null)
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log($"Randomizer.RandomizeHullBaseInfo(): {currentHullBasicInfo} is null");
#endif
                return;
            }

#if DEBUG && UNITY_EDITOR
            Debug.Log($"Randomizer.RandomizeHullBaseInfo(): {currentHullBasicInfo} is not null");
#endif
            List<HullInfo> hullInfoCopy = CreateCleanHullList(currentHullBasicInfo);
            int randomIndex = Random.Range(0, hullInfoCopy.Count);
            RandomizedCombo.Hull.BaseInfo = hullInfoCopy[randomIndex].BaseInfo;
            OnHullBasicInfoRandomized?.Invoke(RandomizedCombo.Hull.BaseInfo, hullInfoCopy);
#if DEBUG && UNITY_EDITOR
            Debug.Log($"Randomizer.RandomizeHullBaseInfo(): After change {PlayerComboHandler.Instance.PlayerCombo.Hull.BaseInfo.HullName}");
#endif
        }

        public void RandomizeTurretSkin()
        {
            if (PlayerComboHandler.Instance.PlayerCombo.Turret.BaseInfo is null || PlayerComboHandler.Instance.PlayerCombo.Turret.BaseInfo.TurretName == TurretBasicInfo.Name.None)
            {
#if DEBUG && UNITY_EDITOR
                Debug.Log("PlayerCombo is null or Turret name is None");
#endif
                return;
            }
            TurretSkin currentTurretSkin = PlayerComboHandler.Instance.PlayerCombo.Turret.Skin;
            List<TurretSkin> turretSkinCopy = TankWiki.Instance.GetTurretSkinsList(PlayerComboHandler.Instance.PlayerCombo.Turret.BaseInfo.TurretName);

            if (currentTurretSkin != null)
            {
                int duplicateIndex = GetExistingTurretItemIndex(turretSkinCopy, currentTurretSkin.GetName);
#if DEBUG && UNITY_EDITOR
                Debug.Log($"Randomizer.RandomizeTurretBaseInfo(): duplicateIndex -> {duplicateIndex}");
#endif
                if (duplicateIndex >= 0)
                {
                    turretSkinCopy.RemoveAt(duplicateIndex);
                }

            }
            RandomizedCombo.Turret.Skin = turretSkinCopy[Random.Range(0, turretSkinCopy.Count)];
            OnTurretSkinRandomized?.Invoke(RandomizedCombo.Turret.Skin);
        }

        public void RandomizeHullSkin()
        {
            if (PlayerComboHandler.Instance.PlayerCombo.Hull.BaseInfo is null || PlayerComboHandler.Instance.PlayerCombo.Hull.BaseInfo.HullName == HullBasicInfo.Name.None)
            {
                return;
            }
            HullSkin currentHullSkin = PlayerComboHandler.Instance.PlayerCombo.Hull.Skin;
            List<HullSkin> hullSkinCopy = TankWiki.Instance.GetHullSkinsList(PlayerComboHandler.Instance.PlayerCombo.Hull.BaseInfo.HullName);

            if (currentHullSkin != null)
            {
                int duplicateIndex = GetExistingHullItemIndex(hullSkinCopy, currentHullSkin.GetName);
                if (duplicateIndex >= 0)
                {
                    hullSkinCopy.RemoveAt(duplicateIndex);
                }
            }
            RandomizedCombo.Hull.Skin = hullSkinCopy[Random.Range(0, hullSkinCopy.Count)];
            OnHullSkinRandomized?.Invoke(RandomizedCombo.Hull.Skin);
        }

        private int GetExistingCoatingItemIndex(List<Coating> coatings, Coating.Name coatingName)
        {
            for (int i = 0; i < coatings.Count; i++)
            {
                if (coatings[i].CoatingName == coatingName)
                {
                    return i;
                }
            }
            return -1;
        }

        private int GetExistingPaintItemIndex(List<Paint> paints, Paint.Name paintName)
        {
            for (int i = 0; i < paints.Count; i++)
            {
                if (paints[i].PaintName == paintName)
                {
                    return i;
                }
            }
            return -1;
        }

        private int GetExistingTurretItemIndex(List<TurretSkin> turretSkins, Skin.Name skinName)
        {
            for (int i = 0; i < turretSkins.Count; i++)
            {
                if (turretSkins[i].GetName == skinName)
                {
                    return i;
                }
            }
            return -1;
        }

        private int GetExistingHullItemIndex(List<HullSkin> hullSkins, Skin.Name skinName)
        {
            for (int i = 0; i < hullSkins.Count; i++)
            {
                if (hullSkins[i].GetName == skinName)
                {
                    return i;
                }
            }
            return -1;
        }

        private int GetExistingTurretItemIndex(List<TurretInfo> turretInfos, TurretBasicInfo.Name turretName)
        {
            for (int i = 0; i < turretInfos.Count; i++)
            {
                if (turretInfos[i].BaseInfo.TurretName == turretName)
                {
                    return i;
                }
            }
            return -1;
        }

        private int GetExistingHullItemIndex(List<HullInfo> hullInfos, HullBasicInfo.Name hullName)
        {
            for (int i = 0; i < hullInfos.Count; i++)
            {
                if (hullInfos[i].BaseInfo.HullName == hullName)
                {
                    return i;
                }
            }
            return -1;
        }

        private void RandomizeModules()
        {
            RandomizeFirstModuleSlot();
            RandomizeSecondModuleSlot();
            RandomizeThirdModuleSlot();
            RandomizeFourthModuleSlot();
            RandomizeFifthModuleSlot();
            RandomizeSixthModuleSlot();
        }

        public void RandomizeFirstModuleSlot() => RandomizeModule(TankPartType.Turret, ModuleType.ActivationType.Active, true);
        public void RandomizeSecondModuleSlot() => RandomizeModule(TankPartType.Turret, ModuleType.ActivationType.Active, false);
        public void RandomizeThirdModuleSlot() => RandomizeModule(TankPartType.Turret, ModuleType.ActivationType.Passive);
        public void RandomizeFourthModuleSlot() => RandomizeModule(TankPartType.Hull, ModuleType.ActivationType.Active, true);
        public void RandomizeFifthModuleSlot() => RandomizeModule(TankPartType.Hull, ModuleType.ActivationType.Active, false);
        public void RandomizeSixthModuleSlot() => RandomizeModule(TankPartType.Hull, ModuleType.ActivationType.Passive);

        private void RandomizeModule(TankPartType tankPartType, ModuleType.ActivationType activationType, bool isFirstActiveModule = true)
        {
            List<Module> moduleCopyList = GetCopyList(tankPartType, activationType);

            Module currentModule = PlayerComboHandler.Instance.PlayerCombo.ModulesArray.GetModule(tankPartType, activationType, isFirstActiveModule);
            if(currentModule != null && moduleCopyList.Contains(currentModule))
            {
                moduleCopyList.Remove(currentModule);
            }
            Module otherModule = PlayerComboHandler.Instance.PlayerCombo.ModulesArray.GetModule(tankPartType, activationType, !isFirstActiveModule);
            if (moduleCopyList.Contains(otherModule))
            {
                moduleCopyList.Remove(otherModule);
            }
            RandomizedCombo.ModulesArray.SetModule(tankPartType, activationType, moduleCopyList[Random.Range(0, moduleCopyList.Count)], isFirstActiveModule);
            OnModuleSlotRandomized?.Invoke(RandomizedCombo.ModulesArray.GetModule(tankPartType, activationType, isFirstActiveModule), RandomizedCombo.ModulesArray.GetModuleIndex(tankPartType, activationType, isFirstActiveModule));
        }

        private List<Module> GetCopyList(TankPartType tankPartType, ModuleType.ActivationType activationType)
        {
            if (tankPartType is TankPartType.Turret)
            {
                if (activationType is ModuleType.ActivationType.Active)
                {
                    return new List<Module>(TankWiki.Instance.ActiveTurretModules);
                }
                else if (activationType is ModuleType.ActivationType.Passive)
                {
                    return new List<Module>(TankWiki.Instance.PassiveTurretModules);
                }
                else
                {
                    Debug.LogError("activationType not recognized");
                    return null;
                }
            }
            else if (tankPartType is TankPartType.Hull)
            {
                if (activationType is ModuleType.ActivationType.Active)
                {
                    return new List<Module>(TankWiki.Instance.ActiveHullModules);
                }
                else if (activationType is ModuleType.ActivationType.Passive)
                {
                    return new List<Module>(TankWiki.Instance.PassiveHullModules);
                }
                else
                {
                    Debug.LogError("activationType not recognized");
                    return null;
                }
            }
            else
            {
                Debug.LogError("TankPartType not recognized");
                return null;
            }
        }

        public void RandomizeAmmunition()
        {
            if (PlayerComboHandler.Instance.PlayerCombo.Turret.BaseInfo.TurretName == TurretBasicInfo.Name.None)
                return;
            //Select ammo list
            AmmunitionListContainer ammoListContainer = TankWiki.Instance.GetAmmunitionListContainer(PlayerComboHandler.Instance.PlayerCombo.Turret.BaseInfo.TurretName);
            List<Ammunition> ammoList = new List<Ammunition>(ammoListContainer.AmmunitionsList);
            //Remove eventual pre-existant ammo
            Ammunition currentAmmo = PlayerComboHandler.Instance.PlayerCombo.Turret.Ammunition;
            if(currentAmmo != null && ammoList.Contains(currentAmmo))
            {
                ammoList.Remove(currentAmmo);
            }
            RandomizedCombo.Turret.Ammunition = ammoList[Random.Range(0, ammoList.Count)];  
            OnAmmunitionSlotRandomized?.Invoke(ammoListContainer, RandomizedCombo.Turret.Ammunition);
        }

        private T RandomizeElementInEnum<T>(T startElement)
        {
            T currentElement = startElement;

            T[] elementsArray = (T[])Enum.GetValues(typeof(T));
            List<T> copyList = new List<T>();
            for (int i = 1; i < elementsArray.Length; i++)
            {
                copyList.Add(elementsArray[i]);
            }

            if (copyList.Contains(currentElement))
            {
                copyList.Remove(currentElement);
            }
            return copyList[Random.Range(0, copyList.Count)];
        }

        private bool RandomizeBool()
        {
            return Random.value > .5f;
        }
    }
}