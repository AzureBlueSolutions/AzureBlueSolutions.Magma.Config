using AzureBlueSolutions.Magma.Config.Models;
using System.Text.Json.Serialization;

namespace AzureBlueSolutions.Magma.Config.Serialization;

[JsonSourceGenerationOptions(
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(MagmaConfig))]
[JsonSerializable(typeof(CodegenOptions))]
[JsonSerializable(typeof(EsbuildOptions))]
[JsonSerializable(typeof(SwcOptions))]
internal sealed partial class MagmaJsonContext : JsonSerializerContext
{
}