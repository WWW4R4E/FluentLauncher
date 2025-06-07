using System.IO.Compression;
using FluentLauncher.Core.Models;
using Tomlyn;
using System.Text.Json.Nodes;
using Tomlyn.Model;


public class ModManageCommand
{
    public static Mod GetModInfoFromJar(string jarPath)
    {
        var modInfo = new Mod
        {
            ModPath = jarPath,
            IsEnabled = Path.GetExtension(jarPath).Equals(".jar")
        };
        using (var archive = new ZipArchive(File.OpenRead(jarPath)))
        {
            var quiltModJson = archive.GetEntry("quilt.mod.json");
            var fabricModJson = archive.GetEntry("fabric.mod.json");
            var forgeModsToml = archive.GetEntry("META-INF/mods.toml");
            var neoForgeModsToml = archive.GetEntry("META-INF/neoforge.mods.toml");
            var mcmodInfo = archive.GetEntry("mcmod.info");

            if (quiltModJson != null)
                modInfo.ModType.Add(ModLoaderType.Quilt);
            if (fabricModJson != null)
                modInfo.ModType.Add(ModLoaderType.Fabric);
            if (forgeModsToml != null || mcmodInfo != null)
                modInfo.ModType.Add(ModLoaderType.Forge);
            if (neoForgeModsToml != null)
                modInfo.ModType.Add(ModLoaderType.NeoForge);

            if (quiltModJson != null)
                return ParseModJson(ref modInfo, quiltModJson, true);
            if (fabricModJson != null)
                return ParseModJson(ref modInfo, fabricModJson, false);

            if (forgeModsToml != null)
                return ParseModsToml(ref modInfo, forgeModsToml);
            if (mcmodInfo != null)
                return ParseForgeMcmodInfo(ref modInfo, mcmodInfo);

            if (neoForgeModsToml != null)
                return ParseModsToml(ref modInfo, neoForgeModsToml);

            modInfo.ModName = Path.GetFileNameWithoutExtension(jarPath);
            return modInfo;
        }
    }

    private static Mod ParseModJson(ref Mod mod, ZipArchiveEntry json, bool isQuilt)
    {
        var jsonContent = json.Open();
        var jsonNode = JsonNode.Parse(jsonContent);

        if (isQuilt)
            jsonNode = jsonNode?["quilt_loader"]?["metadata"];

        if (jsonNode is null)
            throw new InvalidDataException($"Invalid {nameof(jsonContent)}");

        mod.ModName = jsonNode["name"]?.GetValue<string>();
        mod.ModVersion = jsonNode["version"]?.GetValue<string>();
        mod.ModDescription = jsonNode["description"]?.GetValue<string>().TrimEnd('\n').TrimEnd('\r');

        try
        {
            mod.ModAuthor = jsonNode["authors"]
                ?.AsArray()
                .Where(x => x?.GetValue<string>() is not null)
                .Select(x => x?.GetValue<string>()!)
                .ToArray();
        }
        catch
        {
        }

        return mod;
    }

    private static Mod ParseModsToml(ref Mod mod, ZipArchiveEntry toml)
    {
        var stream = toml.Open();
        using (var reader = new StreamReader(stream))
        {
            var tomlContent = reader.ReadToEnd();
            try
            {
                var tomlTable = ((Toml.ToModel(tomlContent)["mods"] as TomlTableArray)?.FirstOrDefault())
                        ?? throw new InvalidDataException("Invalid mods.toml");

                mod.ModName = (tomlTable["displayName"] as string)!;
                mod.ModVersion = tomlTable["version"] as string;
                mod.ModDescription = (tomlTable["description"] as string)?.TrimEnd('\n').TrimEnd('\r');
                if (tomlTable.ContainsKey("authors"))
                {
                    try
                    {
                        mod.ModAuthor = (tomlTable["authors"] as string)?.Split(",").Select(x => x.Trim(' ')).ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error parsing authors: " + e.Message);
                    }
                }
                else
                {
                    mod.ModAuthor = null;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidDataException($"Error parsing mods.toml in {mod.ModPath}: {ex.Message}");
            }
        }

        if (mod.ModVersion == "${file.jarVersion}") mod.ModVersion = null;

        return mod;
    }

    private static Mod ParseForgeMcmodInfo(ref Mod mod, ZipArchiveEntry json)
    {
        using (var stream = json.Open())
        using (var reader = new StreamReader(stream))
        {
            var jsonContent = reader.ReadToEnd();
            var jsonNode =
                JsonNode.Parse(jsonContent.Replace("\u000a", "") ?? "")?.AsArray().FirstOrDefault()
                ?? throw new InvalidDataException("Invalid mcmod.info");

            mod.ModName = jsonNode["name"]?.GetValue<string>();
            mod.ModVersion = jsonNode["version"]?.GetValue<string>();
            mod.ModDescription = jsonNode["description"]?.GetValue<string>().TrimEnd('\n').TrimEnd('\r');
            mod.ModAuthor = (jsonNode["authorList"] ?? jsonNode["authors"])
                ?.AsArray()
                .Where(x => x?.GetValue<string>() is not null)
                .Select(x => x?.GetValue<string>()!)
                .ToArray();
        }

        return mod;
    }
}