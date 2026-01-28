using MinecraftLaunch.Base.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;

namespace LambdaLauncher.Models.Record;

public record LocalConfig
{
    public Language? Language { get; set; }
    public IEnumerable<string> GameRoots { get; set; } = [];
    public string? CurrentGameRoot { get; set; }
    public IEnumerable<Account> Accounts { get; set; } = [];
}
