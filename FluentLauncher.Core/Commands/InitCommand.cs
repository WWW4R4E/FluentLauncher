using System.CommandLine;
using Launcher.Core.Models;
using Launcher.Core.Utils;

namespace Launcher.Core.Commands;

public class InitCommand : Command
{
    private const string ConfigFilePath = "launcher-config.json";

    private readonly Option<string> javaPathOption =
        new(new[] { "--java-path", "-j" }, description: "Path to Java executable");

    private readonly Option<int> memoryOption =
        new(new[] { "--memory", "-m" }, getDefaultValue: () => 1024, description: "Memory allocation in MB");

    private readonly Option<bool> fullscreenOption =
        new(new[] { "--fullscreen", "-f" }, description: "Start launcher in fullscreen mode");

    public InitCommand() : base("init", "Initialize the launcher")
    {
        // 添加命令行参数
        AddOption(javaPathOption);
        AddOption(memoryOption);
        AddOption(fullscreenOption);

        this.SetHandler(async (string javaPath, int memory, bool fullscreen) =>
        {
            Console.WriteLine("Entering interactive configuration mode...");

            // Java路径
            if (string.IsNullOrEmpty(javaPath))
            {
                Console.Write("Enter the path to java.exe (like: C:\\Program Files\\Java\\bin\\java.exe): ");
                javaPath = Console.ReadLine();
            }

            // 内存
            if (memory <= 0)
            {
                Console.Write($"Enter memory allocation in MB (default: {1024}): ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out var parsedMemory))
                    memory = parsedMemory;
            }

            // 全屏设置
            Console.Write("Start launcher in fullscreen? (y/n): ");
            var fullscreenInput = Console.ReadLine().Trim().ToLower();
            fullscreen = fullscreenInput == "y";

            // 构建配置
            var config = new ConfigModel
            {
                LauncherConfig = new LauncherConfig
                {
                    JavaPath = string.IsNullOrEmpty(javaPath) ? new List<string>() : new List<string> { javaPath },
                    Memory = memory,
                    Fullscreen = fullscreen
                },
                UserConfig = new UserProfile
                {
                    uuid = Guid.NewGuid().ToString(),
                    Name = "Steve"
                },
                GameConfig = new List<GameConfig>
                {
                }
            };

            await JsonConfigUtils.SaveToFileAsync(config);
            Console.WriteLine($"Configuration saved to {ConfigFilePath}");
        }, javaPathOption, memoryOption, fullscreenOption);
    }
}