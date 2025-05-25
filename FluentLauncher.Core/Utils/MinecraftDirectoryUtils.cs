namespace FluentLauncher.Core.Utils
{
    public static class MinecraftDirectoryUtils
    {
        public static string GetMinecraftDirectory()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ".minecraft");
        }

        public static string GetVersionsDirectory()
        {
            return Path.Combine(GetMinecraftDirectory(), "versions");
        }

        public static string GetLibrariesDirectory()
        {
            return Path.Combine(GetMinecraftDirectory(), "libraries");
        }

        public static string GetAssetsDirectory()
        {
            return Path.Combine(GetMinecraftDirectory(), "assets");
        }

        public static string GetLogConfigsDirectory()
        {
            return Path.Combine(GetMinecraftDirectory(), "log_configs");
        }
    }
}