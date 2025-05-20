namespace Launcher.Core.Models
{
    public class VersionManifest
    {
        public required string VersionNumber { get; set; }
        public required string ReleaseDate { get; set; }
        public required string DownloadUrl { get; set; }
    }
}