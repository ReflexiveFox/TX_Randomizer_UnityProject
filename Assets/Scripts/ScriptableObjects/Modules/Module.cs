using UnityEngine;

namespace TX_Randomizer
{
    [CreateAssetMenu(fileName = "New module", menuName = "Module")]
    public partial class Module : ScriptableObject
    {
        public Name ModuleName = Name.NotDefined;
        [SerializeField] private Sprite _icon;
        [SerializeField] protected ModuleType _type;
        
        public Sprite Icon => _icon;

        public ModuleType Type => _type;
    }
}