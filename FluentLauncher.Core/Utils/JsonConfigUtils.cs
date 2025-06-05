using System.Text.Json;
using FluentLauncher.Core.Models;

namespace FluentLauncher.Core.Utils;


public static class JsonConfigUtils
{
    //TODO 修改配置文件路径
    static readonly string ConfigFilePath = Path.Combine(AppContext.BaseDirectory, "launcher-config.json");

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

    public static async Task SaveToFileAsync(VersionsJson versionsJson, string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = JsonSerializer.Serialize(versionsJson, AppJsonSerializerContext.Default.VersionsJson);
        await File.WriteAllTextAsync(filePath, json); // 异步写入
    }

public static async Task<Result<VersionJson?>> ReadJsonAsync(string path)
{
    try
    {
        if (!File.Exists(path))
        {
            return Result<VersionJson?>.Failure("文件不存在");
        }

        string json = await File.ReadAllTextAsync(path);
        VersionJson? versionJson = JsonSerializer.Deserialize(json, VersionJsonContext.Default.VersionJson);

        if (versionJson == null)
        {
            return Result<VersionJson?>.Failure("JSON 内容为空或反序列化失败");
        }

        return Result<VersionJson?>.Success(versionJson);
    }
    catch (Exception ex)
    {
        return Result<VersionJson?>.Failure($"无法从路径 {path} 读取 JSON 数据: {ex.Message}");
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
        ConfigModel config = JsonSerializer.Deserialize(json, ConfigModelContext.Default.ConfigModel);
        return config ?? throw new InvalidOperationException("配置文件无效或损坏");
    }
}
