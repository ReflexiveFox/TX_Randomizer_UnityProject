using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TX_Randomizer
{
    public class CoatingSlotAnimation : SlotAnimation
    {
        public event Action<Coating> OnAnimationChanging = delegate { };
        public override event Action OnAnimationFinished = delegate { };

        private void Start()
        {
            Randomizer.Instance.RandomizedCombo.OnCoatingChanged += PlayAnimation;
        }
        private void OnDestroy()
        {
            Randomizer.Instance.RandomizedCombo.OnCoatingChanged -= PlayAnimation;
        }

        protected void PlayAnimation(Coating newCoating)
        {
            if (!IsAnimationPlaying)
            {
                StartCoroutine(SlotAnimation_Coroutine(newCoating));
                IsAnimationPlaying = true;
            }
        }

        protected IEnumerator SlotAnimation_Coroutine(Coating coating)
        {
            List<Coating> coatingList = TankWiki.Instance.Coatings;
            coatingList.RemoveAt(0);

            int previousIndex = -1;

            for (float j = 0f; j < totalDurationTime; j += tickRate)
            {
                int currentIndex = Random.Range(0, coatingList.Count);
                if (currentIndex == previousIndex)
                {
                    float randomValue = Random.value;
                    if (currentIndex == coatingList.Count - 1)
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

                OnAnimationChanging?.Invoke(coatingList[currentIndex]);
                previousIndex = currentIndex;
                yield return new WaitForSeconds(tickRate);
            }
            OnAnimationFinished?.Invoke();
            IsAnimationPlaying = false;
        }
    }
}