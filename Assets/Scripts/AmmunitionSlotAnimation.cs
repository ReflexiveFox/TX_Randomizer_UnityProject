using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TX_Randomizer
{
    public class AmmunitionSlotAnimation : SlotAnimation
    {
        public event Action<Ammunition> OnAnimationChanging = delegate { };
        public override event Action OnAnimationFinished;

        private void Start()
        {
            Randomizer.OnAmmunitionSlotRandomized += PlayAnimation;
        }

        private void OnDestroy()
        {
            Randomizer.OnAmmunitionSlotRandomized -= PlayAnimation;
        }

        protected void PlayAnimation(AmmunitionListContainer container, Ammunition ammunition)
        {
            if (!IsAnimationPlaying)
            {
                StartCoroutine(SlotAnimation_Coroutine(container));
                IsAnimationPlaying = true;
            }
        }
        protected IEnumerator SlotAnimation_Coroutine(AmmunitionListContainer ammoContainer)
        {
            List<Ammunition> ammunitionList = ammoContainer.AmmunitionsList;

            int previousIndex = -1;

            for (float j = 0f; j < totalDurationTime; j += tickRate)
            {
                int currentIndex = Random.Range(0, ammunitionList.Count);
                if (currentIndex == previousIndex)
                {
                    float randomValue = Random.value;
                    if (currentIndex == ammunitionList.Count - 1)
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

                OnAnimationChanging?.Invoke(ammunitionList[currentIndex]);
                previousIndex = currentIndex;
                yield return new WaitForSeconds(tickRate);
            }
            OnAnimationFinished?.Invoke();
            IsAnimationPlaying = false;
        }
    }
}