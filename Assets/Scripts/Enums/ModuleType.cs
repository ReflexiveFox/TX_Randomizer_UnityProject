using System;
using UnityEngine;

namespace TX_Randomizer
{
    [Serializable]
    public class ModuleType
    {
        public enum ActivationType { NotDefined = 0, Active = 1, Passive = 2 }
        public TankPartType TecnologyType;
        public ActivationType ActiveType;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TecnologyType, ActiveType, TecnologyType, ActiveType);
        }

        public static bool operator ==(ModuleType a, ModuleType b)
        {
            return a.TecnologyType == b.TecnologyType && a.ActiveType == b.ActiveType;
        }

        public static bool operator !=(ModuleType a, ModuleType b)
        {
            return !(a == b);
        }
    }
}