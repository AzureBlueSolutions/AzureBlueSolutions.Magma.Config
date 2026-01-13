using AzureBlueSolutions.Magma.Config.Models;
using AzureBlueSolutions.Magma.Config.Serialization;
using System.Text;
using System.Text.Json;

namespace AzureBlueSolutions.Magma.Config.IO;

public static class ConfigLoader
{
    public static (MagmaConfig? config, bool ok, string? error) TryLoad(string path)
    {
        try
        {
            var json = File.ReadAllText(path, Encoding.UTF8);
            var doc = JsonSerializer.Deserialize(json, MagmaJsonContext.Default.MagmaConfig);
            return (doc, doc is not null, doc is null ? "Failed to deserialize magma.json" : null);
        }
        catch (Exception ex)
        {
            return (null, false, ex.Message);
        }
    }

    public static MagmaConfig Load(string path)
    {
        var (config, ok, error) = TryLoad(path);
        if (!ok || config is null) throw new InvalidOperationException(error ?? "Invalid magma.json");
        return config;
    }

    public static void Save(MagmaConfig config, string path)
    {
        var json = JsonSerializer.Serialize(config, MagmaJsonContext.Default.MagmaConfig);
        File.WriteAllText(path, json, new UTF8Encoding(false));
    }

    public static string? TryFind(string startDirectory)
    {
        var dir = Path.GetFullPath(startDirectory);
        for (var i = 0; i < 50; i++)
        {
            var candidate = Path.Combine(dir, "magma.json");
            if (File.Exists(candidate)) return candidate;
            var parent = Directory.GetParent(dir)?.FullName;
            if (string.IsNullOrEmpty(parent) || string.Equals(parent, dir, StringComparison.OrdinalIgnoreCase)) break;
            dir = parent;
        }
        return null;
    }
}