using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TX_Randomizer
{
    public class ModuleSlotAnimation : SlotAnimation
    {
        public override event Action OnAnimationFinished = delegate { };

        [SerializeField] private ModuleType moduleType;

        public void PlaySlotAnimation(Module newModule)
        {
            if (!IsAnimationPlaying)
            {
                StartCoroutine(SlotAnimation_Coroutine(newModule));
                IsAnimationPlaying = true;
            }
        }

        protected IEnumerator SlotAnimation_Coroutine(Module newModule)
        {
            List<Sprite> sprites = new();
            List<Module> modulesList = new();;
            if (newModule.Type == moduleType)
            {
                //Check module type
                if (moduleType.ActiveType is ModuleType.ActivationType.Active)
                {
                    if (moduleType.TecnologyType is TankPartType.Turret)
                    {
                        modulesList = TankWiki.Instance.ActiveTurretModules;
                    }
                    else if (moduleType.TecnologyType is TankPartType.Hull)
                    {
                        modulesList = TankWiki.Instance.ActiveHullModules;
                    }
                }
                else if (moduleType.ActiveType is ModuleType.ActivationType.Passive)
                {
                    if (moduleType.TecnologyType is TankPartType.Turret)
                    {
                        modulesList = TankWiki.Instance.PassiveTurretModules;

                    }
                    else if (moduleType.TecnologyType is TankPartType.Hull)
                    {
                        modulesList = TankWiki.Instance.PassiveHullModules;

                    }
                }

                foreach (Module module in modulesList)
                {
                    sprites.Add(module.Icon);
                }

                int previousIndex = -1;

                for (float j = 0f; j < totalDurationTime; j += tickRate)
                {
                    int currentIndex = Random.Range(0, sprites.Count);
                    if (currentIndex == previousIndex)
                    {
                        float randomValue = Random.value;
                        if (currentIndex == sprites.Count - 1)
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

                    slotImage.sprite = sprites[currentIndex];
                    previousIndex = currentIndex;
                    yield return new WaitForSeconds(tickRate);

                }
                OnAnimationFinished?.Invoke();
                IsAnimationPlaying = false;
            }
        }
    }
}