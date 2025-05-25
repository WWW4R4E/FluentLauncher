
using System.Text.Json.Serialization;

namespace FluentLauncher.Core.Models;

[JsonSerializable(typeof(VersionsJson))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}
[JsonSerializable(typeof(ConfigModel))]
internal partial class ConfigModelContext : JsonSerializerContext
{
}
[JsonSerializable(typeof(VersionJson))]
internal partial class VersionJsonContext : JsonSerializerContext
{
}