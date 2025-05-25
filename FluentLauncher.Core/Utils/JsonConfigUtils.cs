using System.Text.Json;
using FluentLauncher.Core.Models;

namespace FluentLauncher.Core.Utils;


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
        await File.WriteAllTextAsync(ConfigFilePath, json); // 异步写入
    }

    public static async Task<VersionJson?> ReadJsonAsync(string path)
    {
        try
        {
            string json = await File.ReadAllTextAsync(path); // 改为读取文本再反序列化
            VersionJson? versionsJson = JsonSerializer.Deserialize(json, VersionJsonContext.Default.VersionJson);
            return versionsJson;
        }
        catch (Exception ex)
        {
            throw new Exception($"无法从路径 {path} 读取 JSON 数据", ex);
        }
    }

    public static async Task<ConfigModel> LoadFromFile()
    {
        if (!File.Exists(ConfigFilePath))
        {
            // 如果文件不存在，则创建默认配置文件
            await SaveToFileAsync(new ConfigModel
            {
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
            });
        }

        string json = await File.ReadAllTextAsync(ConfigFilePath); // 异步读取
        ConfigModel? config = JsonSerializer.Deserialize(json, ConfigModelContext.Default.ConfigModel);
        return config ?? throw new InvalidOperationException("配置文件无效或损坏");
    }
}
