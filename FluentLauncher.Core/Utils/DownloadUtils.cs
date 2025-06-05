
using FluentLauncher.Core.Models;
namespace FluentLauncher.Core.Utils
{
    public static class DownloadUtils
    {
        private static readonly HttpClient HttpClient = new();

        // 下载文件
        public static async Task DownloadFileAsync(string url, string destinationPath)
        {
            try
            {
                var directory = Path.GetDirectoryName(destinationPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using var response = await HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                await using var contentStream = await response.Content.ReadAsStreamAsync();
                await using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
                await contentStream.CopyToAsync(fileStream);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to download file from {url}", ex);
            }
        }

        // 下载游戏版本
        public static async Task DownloadVersionAsync(VersionInfo version)
        {
            var versionDir = Path.Combine(MinecraftDirectoryUtils.GetVersionsDirectory(), version.Id);
            var versionJsonPath = Path.Combine(versionDir, $"{version.Id}.json");

            // 下载版本 JSON 文件
            await DownloadFileAsync(version.Url, versionJsonPath);

            // 解析 JSON 文件
            var versionDetails = await JsonConfigUtils.ReadJsonAsync(versionJsonPath);

            // 下载客户端文件
            var clientJarPath = Path.Combine(versionDir, $"{version.Id}.jar");
            if (versionDetails != null)
            {
                await DownloadFileAsync(versionDetails.Value.MainClass, clientJarPath);

                // 下载资源文件
                await DownloadAssetsAsync(versionDetails.Value.AssetIndex);

                // 下载依赖库
                await DownloadLibrariesAsync(versionDetails.Value.Libraries);
            }
            else
            {
                throw new Exception($"下载游戏版本失败 {version.Id}");
            }
        }

        // 下载资源文件
        private static async Task DownloadAssetsAsync(AssetIndex assetIndex)
        {
            var assetsDir = MinecraftDirectoryUtils.GetAssetsDirectory();
            var indexPath = Path.Combine(assetsDir, "indexes", $"{assetIndex.Id}.json");

            await DownloadFileAsync(assetIndex.Url, indexPath);

            // TODO: 实现资源文件下载
        }

        // 下载依赖库
        private static async Task DownloadLibrariesAsync(List<Library> libraries)
        {
            var libsDir = MinecraftDirectoryUtils.GetLibrariesDirectory();

            foreach (var library in libraries)
            {
                var libPath = Path.Combine(libsDir, library.Downloads.Artifact.Path);
                await DownloadFileAsync(library.Downloads.Artifact.Url, libPath);
            }
        }
    }
}