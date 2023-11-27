using UnityEngine;
using TMPro;

namespace TX_Randomizer
{
    public abstract class DropdownBase : MonoBehaviour
    {
        protected TMP_Dropdown dropdown;

        protected virtual void Awake()
        {
            dropdown = GetComponent<TMP_Dropdown>();
        }
    }
}