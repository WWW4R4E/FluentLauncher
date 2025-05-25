using System.Diagnostics;
using System.Text;
using FluentLauncher.Core.Models;
using FluentLauncher.Core.Utils;

namespace FluentLauncher.Core.Commands
{
    public class LaunchCommand
    {

        public static async Task<Result<bool>> LaunchGameAsync(GameConfig game)
        {

            var json = await JsonConfigUtils.LoadFromFile();
            GameConfig? selectedGame = game;
            UserProfile userProfile = json.UserConfig;

        
            if (!File.Exists(selectedGame.GamePath))
            {
                return Result<bool>.Failure($"未找到游戏文件：{selectedGame.GamePath}");
            }

            string username = userProfile.Name;
            string gameVersion = selectedGame.GameVersion;
            string memoryArgs = selectedGame.GameArguments.memoryArgs;
            string? accessToken = userProfile.AccessToken ?? string.Empty;
            string javaPath = selectedGame.GameArguments.javaPath ?? "java";

            string uuid = userProfile.uuid;
            string command = MinecraftLauncherUtils.BuildLaunchCommand(gameVersion, accessToken, uuid,
                username, memoryArgs);


            var startInfo = new ProcessStartInfo
            {
                FileName = javaPath,
                Arguments = command,
                UseShellExecute = false,          
                RedirectStandardOutput = true,   
                RedirectStandardError = true,  
                CreateNoWindow = true,           
                WindowStyle = ProcessWindowStyle.Hidden
            };

            try
            {
                using (var process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.EnableRaisingEvents = true;

                    // 读取输出流（可选：绑定事件或直接读取）
                    var output = new StringBuilder();
                    var error = new StringBuilder();

                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            output.AppendLine(e.Data);
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            error.AppendLine(e.Data);
                    };

                    process.Start();

                    // 开始异步读取输出流
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit(); // 等待进程结束

                    // 获取最终输出结果
                    string standardOutput = output.ToString();
                    string standardError = error.ToString();

                    Debug.WriteLine("标准输出:");
                    Debug.WriteLine(standardOutput);

                    if (!string.IsNullOrEmpty(standardError))
                    {
                        Debug.WriteLine("错误输出:");
                        Debug.WriteLine(standardError);
                    }

                    return Result<bool>.Success(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }
    }
}