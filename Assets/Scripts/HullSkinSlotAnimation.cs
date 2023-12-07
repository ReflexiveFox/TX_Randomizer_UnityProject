using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TX_Randomizer
{
    public class HullSkinSlotAnimation : SlotAnimation
    {
        public event Action<HullSkin> OnAnimationChanging = delegate { };
        public override event Action OnAnimationFinished = delegate { };

        private void Start()
        {
            Randomizer.OnHullSkinRandomized += PlayAnimation;
        }

        private void OnDestroy()
        {
            Randomizer.OnHullSkinRandomized -= PlayAnimation;
        }

        protected void PlayAnimation(HullBasicInfo hullBasicInfo)
        {
            if (!IsAnimationPlaying)
            {
                StartCoroutine(SlotAnimation_Coroutine(hullBasicInfo.HullName));
                IsAnimationPlaying = true;
            }
        }

        protected void PlayAnimation(HullSkin finalSkin)
        {
            if (!IsAnimationPlaying)
            {
                StartCoroutine(SlotAnimation_Coroutine(finalSkin.HullOwned));
                IsAnimationPlaying = true;
            }
        }

        protected IEnumerator SlotAnimation_Coroutine(HullBasicInfo.Name hullName)
        {
            List<HullSkin> hullSkinList = TankWiki.Instance.GetHullSkinsList(hullName);

            int previousIndex = -1;

            for (float j = 0f; j < totalDurationTime; j += tickRate)
            {
                int currentIndex = Random.Range(0, hullSkinList.Count);
                if (currentIndex == previousIndex)
                {
                    float randomValue = Random.value;
                    if (currentIndex == hullSkinList.Count - 1)
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

                OnAnimationChanging?.Invoke(hullSkinList[currentIndex]);
                previousIndex = currentIndex;
                yield return new WaitForSeconds(tickRate);
            }
            OnAnimationFinished?.Invoke();
            IsAnimationPlaying = false;
        }
    }
}