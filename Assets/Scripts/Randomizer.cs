using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TX_Randomizer
{
    public class Randomizer : MonoBehaviour
    {
        public static event Action<TurretBasicInfo> OnTurretBasicInfoRandomized = delegate { };
        public static event Action<HullBasicInfo> OnHullBasicInfoRandomized = delegate { };

        public static event Action<TurretSkin> OnTurretSkinRandomized = delegate { };
        public static event Action<HullSkin> OnHullSkinRandomized = delegate { };

        public static event Action<Module, int> OnModuleSlotRandomized = delegate { };

        public static event Action<AmmunitionListContainer, Ammunition> OnAmmunitionSlotRandomized = delegate { }; 

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
            List<Map> mapsCopyList = new List<Map>(TankWiki.instance.Maps);
            
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
            RandomizeTurret();
            RandomizeModules();
            RandomizeHull();
            RandomizeCoating();
            RandomizeAmmunition();
            RandomizePaint();
        }

        private void RandomizeTurret()
        {
            RandomizeTurretBaseInfo();
            RandomizeTurretSkin();
        }

        public void RandomizeCoating()
        {
            Coating currentCoating = PlayerCombo.Turret.Coating;
            List<Coating> coatingListCopy = new List<Coating>(TankWiki.instance.Coatings);
            coatingListCopy.RemoveAt(0);
            int duplicateIndex = GetExistingCoatingItemIndex(coatingListCopy, currentCoating.CoatingName);
            if(duplicateIndex >= 0)
            {
                coatingListCopy.RemoveAt(duplicateIndex);
            }
            PlayerCombo.Turret.Coating = coatingListCopy[Random.Range(0, coatingListCopy.Count)];
        }

        public void RandomizePaint()
        {
            Paint currentPaint = PlayerCombo.Hull.Paint;
            List<Paint> paintListCopy = new List<Paint>(TankWiki.instance.Paints);
            paintListCopy.RemoveAt(0);
            int duplicateIndex = GetExistingPaintItemIndex(paintListCopy, currentPaint.PaintName);
            if (duplicateIndex >= 0)
            {
                paintListCopy.RemoveAt(duplicateIndex);
            }
            PlayerCombo.Hull.Paint = paintListCopy[Random.Range(0, paintListCopy.Count)];
        }

        private void RandomizeHull()
        {
            RandomizeHullBaseInfo();
            RandomizeHullSkin();
        }

        private void RandomizeTurretBaseInfo()
        {
            TurretBasicInfo currentTurretBasicInfo = PlayerCombo.Turret.BaseInfo;
            List<TurretInfo> turretInfoCopy = new List<TurretInfo>(TankWiki.instance.Turrets);

            if (currentTurretBasicInfo != null)
            {
                int duplicateIndex = GetExistingTurretItemIndex(turretInfoCopy, currentTurretBasicInfo.TurretName);
                if (duplicateIndex >= 0)
                {
                    turretInfoCopy.RemoveAt(duplicateIndex);
                }
            }
            PlayerCombo.Turret.BaseInfo = turretInfoCopy[Random.Range(0, turretInfoCopy.Count)].BaseInfo;
            OnTurretBasicInfoRandomized?.Invoke(PlayerCombo.Turret.BaseInfo);
        }

        private void RandomizeHullBaseInfo()
        {
            HullBasicInfo currentHullBasicInfo = PlayerCombo.Hull.BaseInfo;
            List<HullInfo> hullInfoCopy = new List<HullInfo>(TankWiki.instance.Hulls);

            if (currentHullBasicInfo != null)
            {
                int duplicateIndex = GetExistingHullItemIndex(hullInfoCopy, currentHullBasicInfo.HullName);
                if (duplicateIndex >= 0)
                {
                    hullInfoCopy.RemoveAt(duplicateIndex);
                }
            }
            PlayerCombo.Hull.BaseInfo = hullInfoCopy[Random.Range(0, hullInfoCopy.Count)].BaseInfo;
            OnHullBasicInfoRandomized?.Invoke(PlayerCombo.Hull.BaseInfo);
        }

        public void RandomizeTurretSkin()
        {
            if (PlayerCombo.Turret.BaseInfo is null || PlayerCombo.Turret.BaseInfo.TurretName == TurretBasicInfo.Name.None)
            {
                return;
            }
            TurretSkin currentTurretSkin = PlayerCombo.Turret.Skin;
            List<TurretSkin> turretSkinCopy = TankWiki.instance.GetTurretSkinsList(PlayerCombo.Turret.BaseInfo.TurretName);

            if (currentTurretSkin != null)
            {
                int duplicateIndex = GetExistingTurretItemIndex(turretSkinCopy, currentTurretSkin.GetName);
                if (duplicateIndex >= 0)
                {
                    turretSkinCopy.RemoveAt(duplicateIndex);
                }
            }
            PlayerCombo.Turret.Skin = turretSkinCopy[Random.Range(0, turretSkinCopy.Count)];
            OnTurretSkinRandomized?.Invoke(PlayerCombo.Turret.Skin);
        }

        public void RandomizeHullSkin()
        {
            if (PlayerCombo.Hull.BaseInfo is null || PlayerCombo.Hull.BaseInfo.HullName == HullBasicInfo.Name.None)
            {
                return;
            }
            HullSkin currentHullSkin = PlayerCombo.Hull.Skin;
            List<HullSkin> hullSkinCopy = TankWiki.instance.GetHullSkinsList(PlayerCombo.Hull.BaseInfo.HullName);

            if (currentHullSkin != null)
            {
                int duplicateIndex = GetExistingHullItemIndex(hullSkinCopy, currentHullSkin.GetName);
                if (duplicateIndex >= 0)
                {
                    hullSkinCopy.RemoveAt(duplicateIndex);
                }
            }
            PlayerCombo.Hull.Skin = hullSkinCopy[Random.Range(0, hullSkinCopy.Count)];
            OnHullSkinRandomized?.Invoke(PlayerCombo.Hull.Skin);
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

        public void RandomizeTurretSlot()
        {
            RandomizeTurret();
        }

        public void RandomizeHullSlot()
        {
            RandomizeHull();
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

        public void RandomizeFirstModuleSlot()
        {
            RandomizeModule(TankPartType.Turret, ModuleType.ActivationType.Active, true);           
        }
        public void RandomizeSecondModuleSlot()
        {
            RandomizeModule(TankPartType.Turret, ModuleType.ActivationType.Active, false);
        }
        public void RandomizeThirdModuleSlot()
        {
            RandomizeModule(TankPartType.Turret, ModuleType.ActivationType.Passive);
        }
        public void RandomizeFourthModuleSlot()
        {
            RandomizeModule(TankPartType.Hull, ModuleType.ActivationType.Active, true);
        }
        public void RandomizeFifthModuleSlot()
        {
            RandomizeModule(TankPartType.Hull, ModuleType.ActivationType.Active, false);
        }
        public void RandomizeSixthModuleSlot()
        {
            RandomizeModule(TankPartType.Hull, ModuleType.ActivationType.Passive);
        }

        private void RandomizeModule(TankPartType tankPartType, ModuleType.ActivationType activationType, bool isFirstActiveModule = true)
        {
            List<Module> moduleCopyList = GetCopyList(tankPartType, activationType);

            Module currentModule = PlayerCombo.ModulesArray.GetModule(tankPartType, activationType, isFirstActiveModule);
            if(currentModule != null && moduleCopyList.Contains(currentModule))
            {
                moduleCopyList.Remove(currentModule);
            }
            Module otherModule = PlayerCombo.ModulesArray.GetModule(tankPartType, activationType, !isFirstActiveModule);
            if (moduleCopyList.Contains(otherModule))
            {
                moduleCopyList.Remove(otherModule);
            }
            PlayerCombo.ModulesArray.SetModule(tankPartType, activationType, moduleCopyList[Random.Range(0, moduleCopyList.Count)], isFirstActiveModule);
            OnModuleSlotRandomized?.Invoke(PlayerCombo.ModulesArray.GetModule(tankPartType, activationType, isFirstActiveModule), PlayerCombo.ModulesArray.GetModuleIndex(tankPartType, activationType, isFirstActiveModule));
        }

        private List<Module> GetCopyList(TankPartType tankPartType, ModuleType.ActivationType activationType)
        {
            if (tankPartType is TankPartType.Turret)
            {
                if (activationType is ModuleType.ActivationType.Active)
                {
                    return new List<Module>(TankWiki.instance.ActiveTurretModules);
                }
                else if (activationType is ModuleType.ActivationType.Passive)
                {
                    return new List<Module>(TankWiki.instance.PassiveTurretModules);
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
                    return new List<Module>(TankWiki.instance.ActiveHullModules);
                }
                else if (activationType is ModuleType.ActivationType.Passive)
                {
                    return new List<Module>(TankWiki.instance.PassiveHullModules);
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
            if (PlayerCombo.Turret.BaseInfo.TurretName == TurretBasicInfo.Name.None)
                return;
            //Select ammo list
            AmmunitionListContainer ammoListContainer = TankWiki.instance.GetAmmunitionListContainer(PlayerCombo.Turret.BaseInfo.TurretName);
            List<Ammunition> ammoList = new List<Ammunition>(ammoListContainer.AmmunitionsList);
            //Remove eventual pre-existant ammo
            Ammunition currentAmmo = PlayerCombo.Ammunition;
            if(currentAmmo != null && ammoList.Contains(currentAmmo))
            {
                ammoList.Remove(currentAmmo);
            }
            PlayerCombo.Ammunition = ammoList[Random.Range(0, ammoList.Count)];  
            OnAmmunitionSlotRandomized?.Invoke(ammoListContainer, PlayerCombo.Ammunition);
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