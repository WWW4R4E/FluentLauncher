using System.Text.Json.Serialization;

namespace Launcher.Core.Models;

public class ConfigModel
{
    [JsonPropertyName("launcher")]
    public LauncherConfig? LauncherConfig { get; set; }

    [JsonPropertyName("user")]
    public UserProfile? UserConfig { get; set; }

    [JsonPropertyName("game")]
    public List<GameConfig>? GameConfig { get; set; }
    [JsonPropertyName("MinecraftPath")]
    public string? MinecraftPath { get; set; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
        ".minecraft"
    );
}

public class LauncherConfig
{
    [JsonPropertyName("javaPath")]
    public List<string> JavaPath { get; set; } = new();

    [JsonPropertyName("memory")]
    public int Memory { get; set; } = 1024;

    [JsonPropertyName("fullscreen")]
    public bool Fullscreen { get; set; } = false;
}

public class UserProfile
{
    [JsonPropertyName("uuid")]
    public string uuid { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("name")]
    public string Name { get; set; } = "Steve";
    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }
}
public class GameConfig
{

    [JsonPropertyName("gameName")]
    public string GameName { get; set; }
    [JsonPropertyName("gameVersion")]
    public string GameVersion { get; set; }
    [JsonPropertyName("gamePath")]
    public string GamePath { get; set; }
    [JsonPropertyName("javaPath")]
    public string JavaPath { get; set; }
    [JsonPropertyName("memory")]
    public int Memory { get; set; } = 1024;

    [JsonPropertyName("fullscreen")]
    public bool Fullscreen { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public string MemoryArgs => $"-Xms{Memory / 2}M -Xmx{Memory}M";
}
public class VersionJson
{
    [System.Text.Json.Serialization.JsonConstructor]
    public VersionJson(
        GameArguments arguments,
        AssetIndex assetIndex,
        List<Library> libraries,
        string mainClass,
        string id)
    {
        Arguments = arguments;
        AssetIndex = assetIndex;
        Libraries = libraries;
        MainClass = mainClass;
        Id = id;
    }

    [JsonPropertyName("arguments")]
    public GameArguments Arguments { get; set; }

    [JsonPropertyName("assetIndex")]
    public AssetIndex AssetIndex { get; set; }

    [JsonPropertyName("libraries")]
    public List<Library> Libraries { get; set; }
    
    [JsonPropertyName("mainClass")]
    public string MainClass { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public class GameArguments
{
    [System.Text.Json.Serialization.JsonConstructor]
    public GameArguments(
        List<object> game,
        List<object> jvm)
    {
        Game = game;
        Jvm = jvm;
    }

    [JsonPropertyName("game")]
    public List<object> Game { get; set; }

    [JsonPropertyName("jvm")]
    public List<object> Jvm { get; set; }
}

// 添加新的参数类型模型
public class JvmArgument
{
    [JsonPropertyName("rules")]
    public List<Rule> Rules { get; set; }
    
    [JsonPropertyName("value")]
    public object Value { get; set; } // 可以是字符串或数组
}

public class GameArgument
{
    [JsonPropertyName("rules")]
    public List<Rule> Rules { get; set; }
    
    [JsonPropertyName("value")]
    public object Value { get; set; } // 可以是字符串或数组
}

public class AssetIndex
{
    public AssetIndex() {}
    
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("sha1")]
    public string Sha1 { get; set; }
    
    [JsonPropertyName("size")]
    public int Size { get; set; }
}

public class Library
{
    public Library() {}
    
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("downloads")]
    public LibraryDownloads Downloads { get; set; }

    [JsonPropertyName("rules")]
    public List<Rule> Rules { get; set; }
}

public class LibraryDownloads
{
    [JsonPropertyName("artifact")]
    public Artifact Artifact { get; set; }
    
    [JsonPropertyName("classifiers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, Artifact> Classifiers { get; set; }
}

public class Artifact
{
    [JsonPropertyName("path")]
    public string Path { get; set; }
}

public class Rule
{
    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("os")]
    public OsCondition Os { get; set; }
}

public class OsCondition
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("arch")]
    public string Arch { get; set; }
}

// 添加新的日志配置模型
public class LoggingConfig
{
    [JsonPropertyName("client")]
    public LoggingClient Client { get; set; }
}

public class LoggingClient
{
    [JsonPropertyName("argument")]
    public string Argument { get; set; }
    
    [JsonPropertyName("file")]
    public LoggingFile File { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class LoggingFile
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("sha1")]
    public string Sha1 { get; set; }
    
    [JsonPropertyName("size")]
    public int Size { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
}