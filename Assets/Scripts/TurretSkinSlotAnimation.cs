using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TX_Randomizer
{
    public class TurretSkinSlotAnimation : SlotAnimation
    {
        public event Action<TurretSkin> OnAnimationChanging = delegate { };
        public override event Action OnAnimationFinished = delegate { };

        private void Start()
        {
            Randomizer.OnTurretSkinRandomized += PlayAnimation;
        }

        private void OnDestroy()
        {
            Randomizer.OnTurretSkinRandomized -= PlayAnimation;
        }

        protected void PlayAnimation(TurretBasicInfo turretBasicInfo)
        {
            if (!IsAnimationPlaying)
            {
                StartCoroutine(SlotAnimation_Coroutine(turretBasicInfo.TurretName));
                IsAnimationPlaying = true;
            }
        }

        protected void PlayAnimation(TurretSkin finalSkin)
        {
            if (!IsAnimationPlaying)
            {
                StartCoroutine(SlotAnimation_Coroutine(finalSkin.TurretOwned));
                IsAnimationPlaying = true;
            }
        }

        protected IEnumerator SlotAnimation_Coroutine(TurretBasicInfo.Name turretName)
        {
            List<TurretSkin> turretSkinList = TankWiki.Instance.GetTurretSkinsList(turretName);

            int previousIndex = -1;

            for (float j = 0f; j < totalDurationTime; j += tickRate)
            {
                int currentIndex = Random.Range(0, turretSkinList.Count);
                if (currentIndex == previousIndex)
                {
                    float randomValue = Random.value;
                    if (currentIndex == turretSkinList.Count - 1)
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

                OnAnimationChanging?.Invoke(turretSkinList[currentIndex]);
                previousIndex = currentIndex;
                yield return new WaitForSeconds(tickRate);
            }
            OnAnimationFinished?.Invoke();
            IsAnimationPlaying = false;
        }
    }
}