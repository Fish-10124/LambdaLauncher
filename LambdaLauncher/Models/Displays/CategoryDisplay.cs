using LambdaLauncher.Models.Interface;

namespace LambdaLauncher.Models.Displays;

public abstract record CategoryDisplay : IDataDisplay
{
    private string _displayText = null!;

    public required string DisplayText
    {
        get => Utils.ResourceLoader.GetString(_displayText); 
        init => _displayText = value;
    }
}