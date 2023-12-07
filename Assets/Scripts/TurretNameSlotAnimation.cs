using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TX_Randomizer
{
    public class TurretNameSlotAnimation : SlotAnimation
    {
        public event Action<TurretBasicInfo> OnAnimationChanging = delegate { };
        public override event Action OnAnimationFinished = delegate { };

        private void Start()
        {
            Randomizer.OnTurretBasicInfoRandomized += PlayAnimation;
        }

        private void OnDestroy()
        {
            Randomizer.OnTurretBasicInfoRandomized -= PlayAnimation;
        }

        protected void PlayAnimation(TurretBasicInfo turretBasicInfo, List<TurretInfo> itemPool)
        {
            if (IsAnimationPlaying)
            {
                StopAllCoroutines();
            }
            IsAnimationPlaying = true;
            StartCoroutine(SlotAnimation_Coroutine(itemPool));
        }

        protected IEnumerator SlotAnimation_Coroutine(List<TurretInfo> itemPool)
        {
            List<TurretInfo> turretList = itemPool;

            int previousIndex = -1;

            for (float j = 0f; j < totalDurationTime; j += tickRate)
            {
                int currentIndex = Random.Range(0, turretList.Count);
                if (currentIndex == previousIndex)
                {
                    float randomValue = Random.value;
                    if (currentIndex == turretList.Count - 1)
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

                OnAnimationChanging?.Invoke(turretList[currentIndex].BaseInfo);
                previousIndex = currentIndex;
                yield return new WaitForSeconds(tickRate);
            }
            OnAnimationFinished?.Invoke();
            IsAnimationPlaying = false;
        }
    }
}