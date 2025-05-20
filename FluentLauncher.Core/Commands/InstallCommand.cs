using System.CommandLine;

namespace Launcher.Core.Commands
{
    public class InstallCommand : Command
    {
        public InstallCommand() : base("install", "Install the Minecraft game")
        {
            // 可以在这里添加命令的参数和处理逻辑
        }
    }
}