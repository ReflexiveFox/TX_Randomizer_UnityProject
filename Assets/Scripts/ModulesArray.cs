using System;
using UnityEngine;

namespace TX_Randomizer
{
    [System.Serializable]
    public class ModulesArray
    {
        public Module[] Modules = new Module[6];

        public int GetModuleIndex(TankPartType tankPartType, ModuleType.ActivationType activationType, bool isFirstActiveModule = true)
        {
            if(tankPartType is TankPartType.Turret)
            {
                if(activationType is ModuleType.ActivationType.Active)
                {
                    if(isFirstActiveModule)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (activationType is ModuleType.ActivationType.Passive)
                {
                    return 2;
                }
                else
                {
                    Debug.LogError("ActivationType is not defined");
                    return -1;
                }
            }
            else if (tankPartType is TankPartType.Hull)
            {
                if (activationType is ModuleType.ActivationType.Active)
                {
                    if (isFirstActiveModule)
                    {
                        return 3;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else if (activationType is ModuleType.ActivationType.Passive)
                {
                    return 5;
                }
                else
                {
                    Debug.LogError("ActivationType is not defined");
                    return -1;
                }
            }
            else
            {
                Debug.LogError("TankPartType is not defined");
                return -1;
            }
        }

        public Module GetModule(TankPartType tankPartType, ModuleType.ActivationType activationType, bool isFirstActiveModule = true)
        {
            int index = GetModuleIndex(tankPartType, activationType, isFirstActiveModule);
            return Modules[index];
        }

        public void SetModule(TankPartType tankPartType, ModuleType.ActivationType activationType, Module newModule, bool isFirstActiveModule = true)
        {
            int index = GetModuleIndex(tankPartType, activationType, isFirstActiveModule);
            Modules[index] = newModule;
        }
    }
}