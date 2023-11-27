using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TX_Randomizer
{
    public class MapNameDropdown : DropdownContentImage
    {
        Map map;
        [SerializeField] private TextMeshProUGUI mapText;

        private void Start()
        {
            BattleInfo.OnMapChanged += ApplyMapName;
            dropdown.onValueChanged.AddListener(ApplyMapPanel);
            CreateDropdownOptionsList(TankWiki.instance.Maps);
        }

        private void OnDestroy()
        {
            BattleInfo.OnMapChanged -= ApplyMapName;
            dropdown.onValueChanged.RemoveListener(ApplyMapPanel);
        }

        private void ApplyMapPanel(int mapIndex)
        {
            List<Map> copyList = new(TankWiki.instance.Maps);

            for(int i = 0; i < copyList.Count; i++)
            {
                if (i == mapIndex)
                {
                    map = copyList[i];
                    contentImage.sprite = map.Sprite;
                    mapText.text = TankWiki.instance.MapNamesDictionary.GetString(map.MapName);
                }
            }
        }

        private void ApplyMapName()
        {
            map = BattleInfo.Map;
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == TankWiki.instance.MapNamesDictionary.GetString(map.MapName))
                {
                    dropdown.value = i;
                    ApplyMapPanel(i);
                    break;
                }
            }
        }

        private void CreateDropdownOptionsList(List<Map> mapList)
        {
            List<TMP_Dropdown.OptionData> options = new();

            dropdown.ClearOptions();
            foreach (Map map in mapList)
            {
                options.Add(new TMP_Dropdown.OptionData(TankWiki.instance.MapNamesDictionary.GetString(map.MapName)));
            }
            dropdown.AddOptions(options);
        }
    }
}