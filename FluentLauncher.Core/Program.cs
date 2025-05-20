using System.CommandLine;
using Launcher.Core.Commands;

namespace Launcher.Core;
class Program
{
    static async Task<int> Main(string[] args)
    {
        // 创建命令行应用程序
        var rootCommand = new RootCommand("MC Launcher CLI");

        // 添加子命令
        rootCommand.AddCommand(new InitCommand());
        rootCommand.AddCommand(new InstallCommand());
        rootCommand.AddCommand(new LaunchCommand());
        rootCommand.AddCommand(new ScanCommand());

        // 解析命令行参数
        return await rootCommand.InvokeAsync(args);
    }
}
