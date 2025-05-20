
using System.Text.Json.Serialization;

namespace Launcher.Core.Models; 

[JsonSerializable(typeof(VersionJson))]
internal partial class SourceGenerationContext : JsonSerializerContext 
{
}
