using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TX_Randomizer
{
    public abstract class StringDictionary<ElementNameType> : ScriptableObject
    {
        public List<EnumToStringElement<ElementNameType>> Dictionary;

        public virtual string GetString(ElementNameType elementName)
        {
            foreach(EnumToStringElement<ElementNameType> element in Dictionary)
            {
                if(element.KeyElement.ToString() == elementName.ToString())
                {
                    return element.ValueElement;
                }
            }
            Debug.LogError($"String for {elementName} not found");
            return null;
        }
    }
}