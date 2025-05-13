using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.DailyLogin
{
    public class RouletteSpinner
    {
        private const float SpinDurationPerStep = 0.2f;
    
        public async UniTask Spin(VerticalLayoutGroup column, int steps)
        {
            UniTaskCompletionSource completion = new UniTaskCompletionSource();
        
            var items = GetChildItems(column.transform);
            if (items.Count == 0) 
                return;
        
            var spacing = column.spacing;
            var itemHeight = items[0].rect.height;
            var stepDistance = itemHeight + spacing;
            var spinDuration = SpinDurationPerStep * steps;
            var itemCount = items.Count;

            var initialYs = new float[itemCount];
            float bottomMostY = float.MaxValue;
            for (int i = 0; i < itemCount; i++)
            {
                initialYs[i] = items[i].anchoredPosition.y;
                if (initialYs[i] < bottomMostY) 
                    bottomMostY = initialYs[i];
            }

            float cycleDistance = stepDistance * itemCount;

            DOTween.To(() => 0f, value =>
                {
                    float progress = value;
                    float currentDistance = (steps * stepDistance) * progress;

                    for (int i = 0; i < itemCount; i++)
                    {
                        var item = items[i];
                        float effectiveDistance = currentDistance % cycleDistance;
                        float newY = initialYs[i] - effectiveDistance;

                        if (newY < bottomMostY)
                            newY += cycleDistance;

                        var anchoredPosition = item.anchoredPosition;
                        anchoredPosition.y = newY;
                        item.anchoredPosition = anchoredPosition;
                    }
                }, 1f, spinDuration)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() => completion.TrySetResult());
        
        
            await completion.Task;
        }

        private List<RectTransform> GetChildItems(Transform parent)
        {
            var itemList = new List<RectTransform>();
            for (var i = 0; i < parent.childCount; i++)
            {
                var rect = parent.GetChild(i).GetComponent<RectTransform>();
                if (rect != null)
                    itemList.Add(rect);
            }
            return itemList;
        }
    }
}