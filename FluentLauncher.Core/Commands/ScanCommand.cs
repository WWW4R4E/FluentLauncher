using System.CommandLine;
using Launcher.Core.Models;
using Launcher.Core.Utils;

namespace Launcher.Core.Commands
{
    public class ScanCommand : Command
    {
        private const string ConfigFilePath = "launcher-config.json";

        private readonly Option<string> _versionsDirOption = new(new[] { "--versions-dir", "-v" }, 
            description: "Path to .minecraft/versions directory"); 
        public ScanCommand() : base("scan", "Scan local Minecraft versions and add them to config")
        {
            AddOption(_versionsDirOption);

            this.SetHandler(async versionsDir =>
            {
                var config = await JsonConfigUtils.LoadFromFile();
                if (!Directory.Exists(versionsDir))
                {
                    Console.WriteLine("No path specified. Using default path.");
                    if (config.MinecraftPath != null)
                    {
                        versionsDir = Path.Combine(config.MinecraftPath, "versions");
                        Console.WriteLine($"Using path from config: {versionsDir}");
                    }
                    else
                    {
                        Console.WriteLine("No default path.");
                    return;
                    }
                }
                var directories = Directory.GetDirectories(versionsDir);
                foreach (var dir in directories)
                {
                    string versionName = new DirectoryInfo(dir).Name;
                    string jarPath = Path.Combine(dir, $"{versionName}.jar");

                    if (File.Exists(jarPath))
                    {
                        // 创建 GameConfig
                        var gameConfig = new GameConfig
                        {
                            GameName = $"Minecraft {versionName}",
                            GameVersion = versionName,
                            GamePath = jarPath,
                            JavaPath = config.LauncherConfig?.JavaPath?.FirstOrDefault() ?? "java",
                            Memory = config.LauncherConfig?.Memory ?? 1024,
                            Fullscreen = config.LauncherConfig?.Fullscreen ?? false
                        };

                        // 如果尚未存在该版本，则添加
                        if (config.GameConfig == null)
                        {
                            config.GameConfig = new List<GameConfig>();
                        }

                        if (!config.GameConfig.Any(g => g.GameVersion == versionName))
                        {
                            config.GameConfig.Add(gameConfig);
                            Console.WriteLine($"Added version: {versionName}");
                        }
                        else
                        {
                            Console.WriteLine($"Skipped existing version: {versionName}");
                        }
                    }
                }

                // 保存配置
                await JsonConfigUtils.SaveToFileAsync(config);
                Console.WriteLine($"Configuration updated and saved to {ConfigFilePath}");

            }, _versionsDirOption);
        }
    }
}
