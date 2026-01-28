using CommunityToolkit.Mvvm.ComponentModel;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Models.Authentication;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaLauncher.ViewModels;

public partial class AccountManagementModel : ObservableObject
{
    [ObservableProperty]
    public partial bool IsLoading { get; set; } = false;

    [ObservableProperty]
    public partial ObservableCollection<SettingsCardDisplay> AccountDisplays { get; set; } = [];

    public AccountManagementModel()
    {
        _ = ReadAccountsFromLocalConfig();
    }

    public async Task ReadAccountsFromLocalConfig()
    {
        AccountDisplays = [.. await Task.WhenAll(App.LocalConfig.Accounts.Select(GetAccountDisplay)) ];
    }

    private async Task<SettingsCardDisplay> GetAccountDisplay(Account account)
    {
        var description = account.Type switch
        {
            AccountType.Offline => "Offline",
            AccountType.Microsoft => "Microsoft",
            AccountType.Yggdrasil => "Yggdrasil",
            _ => throw new NotImplementedException()
        };

        var skin = await Utils.LoadSkinFromLocalAsync($"{account.Uuid}.png") ?? await Utils.LoadSkinFromLocalAsync();
        // TODO: 裁切图片

        return new SettingsCardDisplay()
        {
            Header = account.Name,
            Description = description,
            Icon = skin,
            Parameter = account
        };
    }
}