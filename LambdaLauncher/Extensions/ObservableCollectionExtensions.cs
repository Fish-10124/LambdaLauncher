using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LambdaLauncher.Extensions;

public static class ObservableCollectionExtensions
{
    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }

    public static void GroupDuplicatesTogether<T, TKey>(this ObservableCollection<T> collection, Func<T, TKey> keySelector, IEqualityComparer<TKey>? keyComparer = null) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(keySelector);

        keyComparer ??= EqualityComparer<TKey>.Default;

        // 记录元素的首次出现顺序和计数
        var order = new List<T>();
        var countDict = new Dictionary<TKey, int>(keyComparer);
        var keyToItemMap = new Dictionary<TKey, T>(keyComparer);

        foreach (var item in collection)
        {
            var key = keySelector(item);
            if (!countDict.TryGetValue(key, out int value))
            {
                value = 0;
                countDict[key] = value;
                order.Add(item);
                keyToItemMap[key] = item;
            }
            countDict[key] = ++value;
        }

        // 构建新顺序
        var newOrder = order.SelectMany(item =>
        {
            var key = keySelector(item);
            return Enumerable.Repeat(keyToItemMap[key], countDict[key]);
        }).ToList();

        // 应用新顺序
        ApplyNewOrder(collection, newOrder, keySelector, keyComparer);
    }

    private static void ApplyNewOrder<T, TKey>(ObservableCollection<T> collection, List<T> newOrder, Func<T, TKey> keySelector, IEqualityComparer<TKey> keyComparer)
    {
        var currentList = collection.ToList();

        for (int targetIndex = 0; targetIndex < newOrder.Count; targetIndex++)
        {
            var targetItem = newOrder[targetIndex];
            var targetKey = keySelector(targetItem);

            // 在当前列表中查找具有相同键的元素
            int currentIndex = -1;
            for (int i = targetIndex; i < currentList.Count; i++)
            {
                var currentKey = keySelector(currentList[i]);
                if (keyComparer.Equals(currentKey, targetKey))
                {
                    currentIndex = i;
                    break;
                }
            }

            if (currentIndex != targetIndex && currentIndex != -1)
            {
                collection.Move(currentIndex, targetIndex);

                // 更新当前列表以反映移动
                var item = currentList[currentIndex];
                currentList.RemoveAt(currentIndex);
                currentList.Insert(targetIndex, item);
            }
        }
    }
}