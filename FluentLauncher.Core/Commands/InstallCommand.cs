using System.Text.Json;
using FluentLauncher.Core.Models;
using FluentLauncher.Core.Utils;


namespace FluentLauncher.Core.Commands
{
    public static class InstallCommand
    {
        private static readonly HttpClient HttpClient = new();
        private const string VersionManifestUrl = "https://piston-meta.mojang.com/mc/game/version_manifest.json";

        public static async Task<Result<VersionsJson>> GetVersionManifestAsync()
        {
            try
            {
                var response = await HttpClient.GetStringAsync(VersionManifestUrl);
              var versionManifest = JsonSerializer.Deserialize(response, AppJsonSerializerContext.Default.VersionsJson);

                return Result<VersionsJson>.Success(versionManifest);
            }
            catch (Exception ex)
            {
                return Result<VersionsJson>.Failure($"获取版本清单失败: {ex.Message}");
            }
        }

        public static async Task<Result<bool>> DownloadMinecraftVersionAsync(string versionId)
        {
            var result = await GetVersionManifestAsync();
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure(result.Error);
            }

            var versionManifest = result.Value;
            var versionInfo = versionManifest.Versions.Find(v => v.Id == versionId);
            if (versionInfo != null)
            {
                try
                {
                    Console.WriteLine($"开始下载版本: {versionId}");
                    var destinationPath = Path.Combine("versions", $"{versionId}.json");
                    Directory.CreateDirectory("versions");
                    await DownloadUtils.DownloadFileAsync(versionInfo.Url, destinationPath);
                    return Result<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    return Result<bool>.Failure($"下载版本失败: {ex.Message}");
                }
            }
            else
            {
                return Result<bool>.Failure($"未找到版本: {versionId}");
            }
        }
    }

}