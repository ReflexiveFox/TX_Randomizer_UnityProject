using System;
using UnityEngine;
using UnityEngine.UI;

namespace TX_Randomizer
{
    [RequireComponent(typeof(Image))]
    public abstract class SlotAnimation : MonoBehaviour
    {
        public abstract event Action OnAnimationFinished;

        [SerializeField, Min(.01f)] protected float tickRate;
        [SerializeField, Min(1f)] protected float totalDurationTime;
        private bool _isAnimationPlaying = false;
        protected Image slotImage;

        protected virtual bool IsAnimationPlaying
        {
            get => _isAnimationPlaying;
            set => _isAnimationPlaying = value;
        }

        private void Awake()
        {
            slotImage = GetComponent<Image>();
        }
    }
}