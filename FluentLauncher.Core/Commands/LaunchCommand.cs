using System.CommandLine;
using System.Diagnostics;
using Launcher.Core.Models;
using Launcher.Core.Utils;

namespace Launcher.Core.Commands
{
    public class LaunchCommand : Command
    {
        GameConfig? selectedGame;
        UserProfile userProfile;
        // LauncherConfig launcherConfig;

        private readonly Option<string> gameOption =
            new(new[] { "--game", "-g" },
                description: "Name of the game to launch (like: Minecraft 1.20.2)");

        public LaunchCommand() : base("launch", "Launch the Minecraft game")
        {
            // 添加可选参数
            AddOption(gameOption);

            // 设置命令处理逻辑
            this.SetHandler(async (context) =>
            {
                string gameName = context.ParseResult.GetValueForOption(gameOption);

                var json = await JsonConfigUtils.LoadFromFile();
                var gameConfigs = json.GameConfig;
                // launcherConfig = json.LauncherConfig;
                userProfile = json.UserConfig;
                if (gameConfigs == null || gameConfigs.Count == 0)
                {
                    Console.WriteLine("No game configuration found. Please download a game.");
                    return;
                }


                if (string.IsNullOrEmpty(gameName))
                {
                    Console.WriteLine("Available games:");
                    foreach (var game in gameConfigs)
                    {
                        Console.WriteLine($"- {game.GameName}");
                    }

                    Console.Write("Please enter the game name you want to launch: ");
                    gameName = Console.ReadLine();
                    if (gameConfigs.Count == 1)
                    {
                        selectedGame = gameConfigs[0];
                    }
                    else
                    {
                        selectedGame = gameConfigs.FirstOrDefault(g =>
                            g.GameName.Equals(gameName, StringComparison.OrdinalIgnoreCase));
                    }
                }
                else
                {
                    Console.WriteLine("Game not found in the configuration.");
                    return;
                }

                if (selectedGame == null)
                {
                    Console.WriteLine($"Game '{gameName}' not found in the configuration.");
                    return;
                }

                // 检查 .jar 文件是否存在
                Console.WriteLine($"Checking if game file exists at {selectedGame.GamePath}");
                if (!File.Exists(selectedGame.GamePath))
                {
                    Console.WriteLine($"Error: Game file not found at {selectedGame.GamePath}");
                    return;
                }

                Console.WriteLine($"Launching {selectedGame.GameName}...");

                string username = userProfile.Name;
                string gameVersion = selectedGame.GameVersion;
                string memoryArgs = selectedGame.MemoryArgs;
                string? accessToken = userProfile.AccessToken ?? string.Empty;
                string javaPath = selectedGame.JavaPath ?? "java";

                string uuid = userProfile.uuid;
                string command = MinecraftLauncherUtils.BuildLaunchCommand(gameVersion, accessToken, uuid,
                    username, memoryArgs);

                var startInfo = new ProcessStartInfo
                {
                    FileName = javaPath,
                    Arguments = command,
                    UseShellExecute = true,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Hidden // 隐藏窗口样式
                };
                try
                {
                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error launching game: {ex.Message}");
                }
            });
        }
    }
}