using LambdaLauncher.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LambdaLauncher.Extensions;

public static class CollectionExtensions
{
    public static IEnumerable<TResult> CartesianProduct<T, TResult>(this IEnumerable<T> source, IEnumerable<T> target, Func<T, T, TResult> combiner)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(combiner);

        return source.SelectMany(s => target.Select(t => combiner(s, t)));
    }

    public static IEnumerable<TResult> CartesianProduct<T, TResult>(this IEnumerable<T> source, IEnumerable<T> target, Func<T?, T?, TResult> combiner, EmptyCartesianBehavior emptyBehavior, T? defaultValue = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(combiner);

        if (source.Any() && target.Any())
        {
            return source.SelectMany(s => target.Select(t => combiner(s, t)));
        }
        return emptyBehavior switch
        {
            EmptyCartesianBehavior.ReturnEmpty => [],
            EmptyCartesianBehavior.ReturnSource => source.Select(s => combiner(s, defaultValue)),
            EmptyCartesianBehavior.ReturnTarget => target.Select(t => combiner(defaultValue, t)),
            EmptyCartesianBehavior.ReturnSingle => source.Select(s => combiner(s, defaultValue)).Concat(target.Select(t => combiner(defaultValue, t))),
            _ => throw new NotImplementedException()
        };
    }

    public static IEnumerable<T> OrderByList<T>(this IEnumerable<T> source, IEnumerable<T> orderList, MissingItemsPlacement missingItemsPlacement = MissingItemsPlacement.AtEnd) where T : notnull
    {
        return source.OrderByList(orderList, x => x, missingItemsPlacement);
    }

    public static IEnumerable<T> OrderByList<T, TKey>(this IEnumerable<T> source, IEnumerable<TKey> orderList, Func<T, TKey> keySelector, MissingItemsPlacement missingItemsPlacement = MissingItemsPlacement.AtEnd) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(orderList);
        ArgumentNullException.ThrowIfNull(keySelector);

        // 创建顺序字典
        var orderDict = new Dictionary<TKey, int>();
        int index = 0;
        foreach (var key in orderList)
        {
            if (!orderDict.ContainsKey(key))
            {
                orderDict[key] = index++;
            }
        }

        return [.. source.OrderBy(item =>
            {
                var key = keySelector(item);
                if (orderDict.TryGetValue(key, out int orderIndex))
                {
                    return orderIndex;
                }

                return missingItemsPlacement == MissingItemsPlacement.AtStart ? int.MinValue + index : int.MaxValue;
            }).ThenBy(item => keySelector(item))];
    }

    public static IEnumerable<GroupInfoList<T>> GroupBy<T, TKey>(this IEnumerable<T> collection, Func<T, IEnumerable<TKey>> keySelector) where TKey : notnull
    {
        // 参数检查
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(keySelector);

        var keyToGroupMap = new Dictionary<TKey, GroupInfoList<T>>();
        foreach (var item in collection)
        {
            var keys = keySelector(item);
            if (!keys.Any()) continue;

            foreach (var key in keys)
            {
                if (!keyToGroupMap.TryGetValue(key, out var group))
                {
                    group = new(key);
                    keyToGroupMap[key] = group;
                }
                group.Add(item);
            }
        }
        return keyToGroupMap.Values;
    }
}

public enum MissingItemsPlacement
{
    /// <summary>
    /// 不在顺序列表中的元素放在开头
    /// </summary>
    AtStart,

    /// <summary>
    /// 不在顺序列表中的元素放在末尾
    /// </summary>
    AtEnd
}

public enum EmptyCartesianBehavior
{
    /// <summary>
    /// 返回空集合（数学上的标准行为）
    /// </summary>
    ReturnEmpty,

    /// <summary>
    /// 返回第一个集合（使用默认值填充第二个参数）
    /// </summary>
    ReturnSource,

    /// <summary>
    /// 返回第二个集合（使用默认值填充第一个参数）
    /// </summary>
    ReturnTarget,

    /// <summary>
    /// 返回两个集合的并集（分别使用默认值填充缺失部分）
    /// </summary>
    ReturnSingle
}