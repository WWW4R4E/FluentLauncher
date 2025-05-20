namespace FluentLauncher.Models;

public class GameData
{
    public string GameName { get; set; }
    public string GameVersion { get; set; }
    public bool IsAddMod { get; set; }
    public ModLoaders ModLoaders { get; set; }
}