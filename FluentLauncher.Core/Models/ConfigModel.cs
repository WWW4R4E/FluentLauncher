using System.Text.Json.Serialization;

namespace FluentLauncher.Core.Models;

#region 配置文件

public class ConfigModel
{
    [JsonPropertyName("user")] public UserProfile? UserConfig { get; set; }

    [JsonPropertyName("game")] public List<GameConfig>? GameConfig { get; set; }

    [JsonPropertyName("MinecraftPath")]
    public string? MinecraftPath { get; set; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        ".minecraft"
    );
    [JsonPropertyName("jdkPathGroups")]
    public List<JavaPath> JavaPathGroup { get; set; }
}

public class UserProfile
{
    [JsonPropertyName("uuid")] public string uuid { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("name")] public string Name { get; set; } = "Steve";
    [JsonPropertyName("accessToken")] public string? AccessToken { get; set; }
}

public class GameConfig
{
    [JsonPropertyName("gameName")] public string GameName { get; set; }
    [JsonPropertyName("gameVersion")] public string GameVersion { get; set; }
    [JsonPropertyName("gamePath")] public string GamePath { get; set; }
    [JsonPropertyName("modManage")] public List<Mod> Mods { get; set; }
    [JsonPropertyName("gameSave")] public List<GameSave> GameSaves { get; set; }
    [JsonPropertyName("GameArguments")] public GameArgument GameArguments { get; set; }
}

public class GameSave
{
    [JsonPropertyName("saveName")] public string SaveName { get; set; }

    [JsonPropertyName("savePath")] public string SavePath { get; set; }

    [JsonPropertyName("saveTime")] public DateTime SaveTime { get; set; }
}

public class Mod
{
    [JsonPropertyName("modName")] public string ModName { get; set; }
    [JsonPropertyName("modVersion")] public string ModVersion { get; set; }
    [JsonPropertyName("modPath")] public string ModPath { get; set; }
    [JsonPropertyName("modType")] public string ModType { get; set; } // e.g., "Forge", "Fabric", etc.
}

public class GameArgument
{
    [JsonPropertyName("fullScrean")] public bool FullScrean { get; set; } = false;
    [JsonPropertyName("memoryArgs")] public string memoryArgs { get; set; } = "-Xmx512M -Xms1024M";
    [JsonPropertyName("javaPath")] public string javaPath { get; set; } = "java";
    [JsonPropertyName("gameSize")] public Tuple<uint, uint> GameSize { get; set; } = new Tuple<uint, uint>(1280, 720);
}
public class JavaPath
{

    [JsonPropertyName("path")] public string Path { get; set; }
    [JsonPropertyName("version")] public string Version { get; set; }
}

#endregion

#region 下载json

public class VersionsJson
{
    [JsonPropertyName("latest")] public Latest Latest { get; set; }

    [JsonPropertyName("versions")] public List<VersionInfo> Versions { get; set; }
}

public class Latest
{
    [JsonPropertyName("release")] public string Release { get; set; }

    [JsonPropertyName("snapshot")] public string Snapshot { get; set; }
}

public class VersionInfo
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("time")] public DateTime Time { get; set; }

    [JsonPropertyName("releaseTime")] public DateTime ReleaseTime { get; set; }
}

#endregion

#region 版本json

public class VersionJson
{
    [JsonPropertyName("arguments")] public Arguments Arguments { get; set; }
    [JsonPropertyName("assetIndex")] public AssetIndex AssetIndex { get; set; }
    [JsonPropertyName("assets")] public string Assets { get; set; }
    [JsonPropertyName("compliancelevel")] public int ComplianceLevel { get; set; }
    [JsonPropertyName("downloads")] public Downloads Downloads { get; set; }
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("javaVersion")] public JavaVersion JavaVersion { get; set; }
    [JsonPropertyName("libraries")] public List<Library> Libraries { get; set; }
    [JsonPropertyName("logging")] public LoggingConfig Logging { get; set; }
    [JsonPropertyName("mainClass")] public string MainClass { get; set; }
    [JsonPropertyName("releaseTime")] public string ReleaseTime { get; set; }
    [JsonPropertyName("time")] public string Time { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; }
}

public class Downloads
{
    [JsonPropertyName("client")] public DowenloadItem Client { get; set; }
    [JsonPropertyName("client_mappings")] public DowenloadItem ClientMappings { get; set; }
    [JsonPropertyName("server")] public DowenloadItem Server { get; set; }
    [JsonPropertyName("server_mappings")] public DowenloadItem ServerMappings { get; set; }
}

public class DowenloadItem
{
    [JsonPropertyName("sha1")] public string Sha1 { get; set; }
    [JsonPropertyName("size")] public int Size { get; set; }
    [JsonPropertyName("url")] public string Url { get; set; }
}

public class JavaVersion
{
    [JsonPropertyName("component")] public string Component { get; set; }

    [JsonPropertyName("majorVersion")] public int MajorVersion { get; set; }
}

public class LoggingConfig
{
    [JsonPropertyName("client")] public LoggingClient Client { get; set; }
}

public class LoggingClient
{
    [JsonPropertyName("argument")] public string Argument { get; set; }
    [JsonPropertyName("file")] public LoggingFile File { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; }
}

public class LoggingFile
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("sha1")] public string Sha1 { get; set; }

    [JsonPropertyName("size")] public int Size { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }
}

public class DownloadArtifact
{
    [JsonPropertyName("sha1")] public string Sha1 { get; set; }

    [JsonPropertyName("size")] public int Size { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("path")] public string Path { get; set; }
}

public class LibraryDownloads
{
    [JsonPropertyName("artifact")] public DownloadArtifact Artifact { get; set; }
}

public class Library
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("downloads")] public LibraryDownloads Downloads { get; set; }
}

public class Arguments
{
    [JsonPropertyName("game")] public List<string> GameArguments { get; set; }

    [JsonPropertyName("jvm")] public List<string> JvmArguments { get; set; }
}

public class AssetIndex
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("sha1")] public string Sha1 { get; set; }

    [JsonPropertyName("size")] public int Size { get; set; }
    [JsonPropertyName("totalSize")] public int TotalSize { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }
}

#endregion