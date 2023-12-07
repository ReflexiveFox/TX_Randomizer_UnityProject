using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TX_Randomizer
{
    [RequireComponent(typeof(Image))]
    public abstract class SkinImage : MonoBehaviour
    {
        [SerializeField] protected Sprite noneSprite;
        protected Image skinImage;

        protected virtual void Awake()
        {
            skinImage = GetComponent<Image>();
        }
    }
}