using LambdaLauncher.Models.Record;
using System;
using System.Collections.Generic;

namespace LambdaLauncher.Services;

public class BreadcrumbService
{
    public event Action<IReadOnlyList<PageHeader>>? BreadcrumbChanged;

    public void SetHeader(params PageHeader[] items)
    {
        BreadcrumbChanged?.Invoke(items);
    }
}