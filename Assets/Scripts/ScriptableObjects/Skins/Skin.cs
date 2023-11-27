using UnityEngine;

namespace TX_Randomizer
{
    public abstract partial class Skin : ScriptableObject
    {
        [SerializeField] protected Sprite _sprite;
        [SerializeField] protected Name _skinName;
        [SerializeField] protected TankPartType _tankPartType;
        
        public TankPartType TankPartType => _tankPartType;

        public Name GetName => _skinName;

        public Sprite Sprite => _sprite;
    }
}