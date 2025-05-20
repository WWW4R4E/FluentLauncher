using Launcher.Core.Models;
using System.Text.Json;

namespace Launcher.Core.Utils;

public static class MinecraftLauncherUtils
{
    /// <summary>
    /// 获取 Minecraft 游戏目录
    /// </summary>
    public static string GetGameDirectory()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
    }

    /// <summary>
    /// 获取指定版本的类路径（包含所有依赖库和主 JAR 文件）
    /// </summary>
    public static string GetClassPath(string version, string librariesDir, string versionsDir)
    {
        string versionJarPath = Path.Combine(versionsDir, version, $"{version}.jar");
        List<string> libraryPaths = GetLibraryPaths(version, librariesDir, versionsDir);

        string classPath = string.Join(";", libraryPaths);
        return $"{classPath};{versionJarPath}";
    }

    /// <summary>
    /// 获取依赖库路径列表
    /// </summary>
    private static List<string> GetLibraryPaths(string version, string librariesDir, string versionsDir)
    {
        var libraryPaths = new List<string>();
        string versionJsonPath = Path.Combine(versionsDir, version, $"{version}.json");

        if (!File.Exists(versionJsonPath))
        {
            Console.WriteLine($"版本 JSON 文件不存在：{versionJsonPath}");
            return libraryPaths;
        }

        try
        {
            var json = File.ReadAllText(versionJsonPath);
            var versionJson = JsonSerializer.Deserialize(
                json, 
                SourceGenerationContext.Default.VersionJson // 使用生成的上下文
            );

            foreach (var library in versionJson.Libraries)
            {
                // 添加库规则过滤逻辑（根据当前代码逻辑保留）
                if (library.Downloads?.Artifact != null)
                {
                    string[] parts = library.Name.Split(':');
                    if (parts.Length == 3)
                    {
                        string path = Path.Combine(parts[0].Replace('.', Path.DirectorySeparatorChar),
                            parts[1], parts[2], $"{parts[1]}-{parts[2]}.jar");
                        string fullPath = Path.Combine(librariesDir, path);
                        if (File.Exists(fullPath))
                        {
                            libraryPaths.Add(fullPath);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"解析库文件失败：{ex.Message}");
        }
        return libraryPaths;
    }


    /// <summary>
    /// 获取资源索引 ID
    /// </summary>
    
    public static string GetAssetIndex(string version, string versionsDir)
    {
        
        string versionJsonPath = Path.Combine(versionsDir, version, $"{version}.json");

        if (!File.Exists(versionJsonPath))
        {
            Console.WriteLine($"版本 JSON 文件不存在：{versionJsonPath}");
            return "17";
        }

        try
        {
            var json = File.ReadAllText(versionJsonPath);
            var versionJson = JsonSerializer.Deserialize(
                json, 
                SourceGenerationContext.Default.VersionJson // 使用生成的上下文
            );
            return versionJson?.AssetIndex?.Id ?? "17";
        }
        catch
        {
            return "17";
        }
    }


    /// <summary>
    /// 生成完整的 Minecraft 启动命令
    /// </summary>
    public static string BuildLaunchCommand(
        string version,
        string? accessToken,
        string uuid,
        string username,
        string memoryArgs = "-Xms512M -Xmx2G")
    {
        string gameDir = GetGameDirectory();
        string assetsDir = Path.Combine(gameDir, "assets");
        string librariesDir = Path.Combine(gameDir, "libraries");
        string versionsDir = Path.Combine(gameDir, "versions");
        string nativesDir = Path.Combine(versionsDir, version, "natives");
        string assetIndex = GetAssetIndex(version, versionsDir);
        string classPath = GetClassPath(version, librariesDir, versionsDir);
        return $" {memoryArgs} " +
               $"-Dminecraft.client.jar=\"{Path.Combine(versionsDir, version, $"{version}.jar")}\" " +
               $"-Dlog4j2.formatMsgNoLookups=true " +
               $"-Djava.library.path=\"{nativesDir}\" " +
               $"-Dminecraft.launcher.brand=offline-launcher " +
               $"-Dminecraft.launcher.version=1.0 " +
               $"-cp \"{classPath}\" " +
               $"net.minecraft.client.main.Main " +
               $"--username {username} " +
               $"--version {version} " +
               $"--gameDir \"{gameDir}\" " +
               $"--assetsDir \"{assetsDir}\" " +
               $"--assetIndex {assetIndex} " +
               $"--uuid {uuid} " +
               $"--accessToken {accessToken} " +
               $"--userType offline " +
               $"--versionType release " +
               $"--fullscreen " +
               $"--width 854 " +
               $"--height 480";
    }
}