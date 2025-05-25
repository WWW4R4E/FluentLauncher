using FluentLauncher.Core.Models;
using FluentLauncher.Core.Utils;

namespace FluentLauncher.Core.Commands
{
    public class ScanCommand
    {
        public async Task<Result<bool>> ScanVersionsAsync(string? versionsDir = null)
        {
            var config = await JsonConfigUtils.LoadFromFile();
            if (!Directory.Exists(versionsDir))
            {
                if (config.MinecraftPath != null)
                {
                    versionsDir = Path.Combine(config.MinecraftPath, "versions");
                }
                else
                {
                    return Result<bool>.Failure("未指定版本目录且未找到默认路径");
                }
            }

            var directories = Directory.GetDirectories(versionsDir);
            if (directories.Length == 0)
            {
                return Result<bool>.Failure($"未在目录 {versionsDir} 中找到任何游戏版本");
            }

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
                        // TODO 配置初始化
                        Mods = new List<Mod>(),
                        GameSaves = new List<GameSave>(),
                        GameArguments = new GameArgument()
                    };
                    if (config.GameConfig != null && !config.GameConfig.Any(g => g.GameVersion == versionName))
                    {
                        config.GameConfig.Add(gameConfig);
                    }
                }
            }

            // 保存配置
            await JsonConfigUtils.SaveToFileAsync(config);
            return Result<bool>.Success(true);
        }
    }
}
