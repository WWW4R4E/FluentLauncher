using FluentLauncher.Core.Models;
using FluentLauncher.Core.Utils;

namespace FluentLauncher.Core.Commands;

public static class InitCommand
{
    public static async Task InitializeAsync(List<JavaPath> jdkPaths, string mineCraftPath, string? uuid , string? username, string? accesstoken )
    {
        var config = new ConfigModel
        {
            UserConfig = new UserProfile
            {
                uuid = uuid ?? Guid.NewGuid().ToString(),
                Name = username ?? "Steve",
                AccessToken = accesstoken
            },
            GameConfig = new List<GameConfig>(),
            MinecraftPath = mineCraftPath,
            JavaPathGroup = jdkPaths
        }; 
        await JsonConfigUtils.SaveToFileAsync(config);
    }
}