using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.DailyLogin
{
    public class RouletteSpinResultCalculator
    {
        private List<int> _rewards;

        public List<int> GetTargetItemIndexes(VerticalLayoutGroup[] columns)
        {
            List<int> targetIndexes = new ();
            _rewards = new();
        
            for (int i = 0; i < columns.Length; i++)
            {
                var column = columns[i];
            
                int index = Random.Range(0, column.transform.childCount);
                targetIndexes.Add(index);
            
                var child = column.transform.GetChild(index);
                int reward = child.GetComponent<RouletteItemView>().RouletteItemModel.BaseReward;
                _rewards.Add(reward);
            }
        
            return targetIndexes;
        }

        public int CalculateCoinReward()
        {
            int totalReward = 0;

            int streak = 0;
            int highestReward = _rewards.Max();

            for (int i = 0; i < _rewards.Count; i++)
            {
                int reward = _rewards[i];
                if (reward == highestReward)
                {
                    streak++;
                    totalReward += reward * streak;
                }
            }
        
            return totalReward;
        }
    }
}
