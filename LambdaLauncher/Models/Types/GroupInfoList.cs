using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LambdaLauncher.Models.Types;

public partial class GroupInfoList<T> : ObservableCollection<T>
{
    public object Key { get; set; }

    public GroupInfoList(object key)
    {
        Key = key;
    }

    public GroupInfoList(object key, IEnumerable<T> collection) : base(collection)
    {
        Key = key;
    }
}