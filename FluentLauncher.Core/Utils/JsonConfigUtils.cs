using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Launcher.Core.Models;

namespace Launcher.Core.Utils;

[JsonSerializable(typeof(ConfigModel))]
internal partial class ConfigModelContext : JsonSerializerContext
{
}

public static class JsonConfigUtils
{
    static readonly string ConfigFilePath = "launcher-config.json";

    internal static async Task SaveToFileAsync(ConfigModel config)
    {
        var directory = Path.GetDirectoryName(ConfigFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = JsonSerializer.Serialize(config, ConfigModelContext.Default.ConfigModel);
        System.Console.WriteLine(json);
        File.WriteAllText(ConfigFilePath, json);
    }

    public static async Task<ConfigModel> LoadFromFile()
    {
        if (!File.Exists(ConfigFilePath))
        {
            // 如果文件不存在，则创建文件
            await SaveToFileAsync(new ConfigModel
                {
                    LauncherConfig = new LauncherConfig
                    {
                        Memory = 1024,
                        Fullscreen = false
                    },
                    UserConfig = new UserProfile
                    {
                        uuid = Guid.NewGuid().ToString(),
                        Name = "Steve"
                    },
                    GameConfig = new List<GameConfig>(),
                    MinecraftPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        ".minecraft"
                    )
                }
            );
        }

        string json = File.ReadAllText(ConfigFilePath);
        ConfigModel config = JsonSerializer.Deserialize<ConfigModel>(json, ConfigModelContext.Default.ConfigModel);
        return config ?? throw new InvalidOperationException("Invalid config file.");
    }
}