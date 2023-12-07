using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TX_Randomizer
{
    public class PaintSlotAnimation : SlotAnimation
    {
        public event Action<Paint> OnAnimationChanging = delegate { };
        public override event Action OnAnimationFinished = delegate { };

        private void Start()
        {
            Randomizer.Instance.RandomizedCombo.OnPaintChanged += PlayAnimation;
        }

        private void OnDestroy()
        {
            Randomizer.Instance.RandomizedCombo.OnPaintChanged -= PlayAnimation;
        }

        protected void PlayAnimation(Paint _)
        {
            if (!IsAnimationPlaying)
            {
                StartCoroutine(SlotAnimation_Coroutine());
                IsAnimationPlaying = true;
            }
        }

        protected IEnumerator SlotAnimation_Coroutine()
        {
            List<Paint> paintList = TankWiki.Instance.Paints;
            paintList.RemoveAt(0);

            int previousIndex = -1;

            for (float j = 0f; j < totalDurationTime; j += tickRate)
            {
                int currentIndex = Random.Range(0, paintList.Count);
                if (currentIndex == previousIndex)
                {
                    float randomValue = Random.value;
                    if (currentIndex == paintList.Count - 1)
                    {
                        currentIndex -= 1;
                    }
                    else if (currentIndex == 0)
                    {
                        currentIndex += 1;
                    }
                    else
                    {
                        currentIndex = randomValue > .5f ? currentIndex + 1 : currentIndex - 1;
                    }
                }

                OnAnimationChanging?.Invoke(paintList[currentIndex]);
                previousIndex = currentIndex;
                yield return new WaitForSeconds(tickRate);
            }
            OnAnimationFinished?.Invoke();
            IsAnimationPlaying = false;
        }
    }
}