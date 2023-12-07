using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TX_Randomizer
{
    public class HullNameSlotAnimation : SlotAnimation
    {
        public event Action<HullBasicInfo> OnAnimationChanging = delegate { };
        public override event Action OnAnimationFinished = delegate { };

        private void Start()
        {
            Randomizer.OnHullBasicInfoRandomized += PlayAnimation;
        }

        private void OnDestroy()
        {
            Randomizer.OnHullBasicInfoRandomized -= PlayAnimation;
        }

        protected void PlayAnimation(HullBasicInfo hullBasicInfo, List<HullInfo> itemPool)
        {
            if (IsAnimationPlaying)
            {
                StopAllCoroutines();
            }
            IsAnimationPlaying = true;
            StartCoroutine(SlotAnimation_Coroutine(itemPool));
        }

        protected IEnumerator SlotAnimation_Coroutine(List<HullInfo> itemPool)
        {
            List<HullInfo> hullList = itemPool;

            int previousIndex = -1;

            for (float j = 0f; j < totalDurationTime; j += tickRate)
            {
                int currentIndex = Random.Range(0, hullList.Count);
                if (currentIndex == previousIndex)
                {
                    float randomValue = Random.value;
                    if (currentIndex == hullList.Count - 1)
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

                OnAnimationChanging?.Invoke(hullList[currentIndex].BaseInfo);
                previousIndex = currentIndex;
                yield return new WaitForSeconds(tickRate);
            }
            OnAnimationFinished?.Invoke();
            IsAnimationPlaying = false;
        }
    }
}