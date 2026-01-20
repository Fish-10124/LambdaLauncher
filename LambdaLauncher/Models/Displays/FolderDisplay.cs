using System;
using System.IO;

namespace LambdaLauncher.Models.Displays;

public record FolderDisplay
{
    public readonly string DefaultGameFolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\.minecraft";

    public string FolderName => FullPath == DefaultGameFolderPath ? Utils.ResourceLoader.GetString("DefaultGameFolder") : Path.GetFileName(FullPath)!;
    public required string FullPath { get; set; }
}
