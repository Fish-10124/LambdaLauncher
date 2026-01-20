using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;

namespace LambdaLauncher.Models.Record;

public record LocalConfig
{
    public required Language Language { get; set; }

    public required IEnumerable<string> GameRoots { get; set; }
    public required string CurrentGameRoot { get; set; }
}
