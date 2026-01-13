using AzureBlueSolutions.Magma.Config.IO;
using System.Text.Json.Serialization;

namespace AzureBlueSolutions.Magma.Config.Models;

/// <summary>
/// Root configuration for Magma, including code generation and compiler settings.
/// </summary>
public sealed class MagmaConfig
{
    /// <summary>
    /// Optional project name used for identification.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Optional project version (e.g., "1.0.0").
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    /// Code generation output locations and related options.
    /// </summary>
    [JsonPropertyName("codegen")]
    public CodegenOptions Codegen { get; set; } = new();

    /// <summary>
    /// The active compiler to use for building scripts.
    /// </summary>
    [JsonPropertyName("compiler")]
    [JsonConverter(typeof(JsonStringEnumConverter<MagmaCompilerKind>))]
    public MagmaCompilerKind Compiler { get; set; } = MagmaCompilerKind.Esbuild;

    /// <summary>
    /// Options specific to esbuild (used when <see cref="Compiler"/> is <see cref="MagmaCompilerKind.Esbuild"/>).
    /// </summary>
    [JsonPropertyName("esbuild")]
    public EsbuildOptions? Esbuild { get; set; }

    /// <summary>
    /// Options specific to SWC (used when <see cref="Compiler"/> is <see cref="MagmaCompilerKind.Swc"/>).
    /// </summary>
    [JsonPropertyName("swc")]
    public SwcOptions? Swc { get; set; }

    /// <summary>
    /// Relative path to the node_modules directory used by Magma.
    /// </summary>
    [JsonPropertyName("nodeModulesDir")]
    public string? NodeModulesDir { get; set; } = ".magma/node_modules";

    /// <summary>
    /// Gets the compiler options for the currently selected <see cref="Compiler"/>.
    /// </summary>
    /// <returns>The active compiler options, or <c>null</c> if none are applicable.</returns>
    public ICompilerOptions? GetActiveCompilerOptions()
        => Compiler switch
        {
            MagmaCompilerKind.Esbuild => Esbuild,
            MagmaCompilerKind.Swc => Swc,
            _ => null
        };

    /// <summary>
    /// Resolves the absolute path to the generated code root.
    /// </summary>
    /// <param name="projectRoot">Absolute or relative project root.</param>
    /// <returns>Absolute path to the generated code root.</returns>
    public string GetGeneratedRoot(string projectRoot)
        => PathResolver.GetAbsolutePath(projectRoot, Codegen.OutputFolder ?? ".magma/obj/generated");

    /// <summary>
    /// Resolves the absolute path to the TypeScript output root.
    /// </summary>
    /// <param name="projectRoot">Absolute or relative project root.</param>
    /// <returns>Absolute path to the TypeScript output root.</returns>
    public string GetTypescriptOutputRoot(string projectRoot)
        => PathResolver.GetAbsolutePath(projectRoot, Codegen.TypescriptOutput ?? ".magma/obj/ts");
}

/// <summary>
/// Options controlling code generation output directories.
/// </summary>
public sealed class CodegenOptions
{
    /// <summary>
    /// Root folder where generated code is emitted.
    /// </summary>
    [JsonPropertyName("outputFolder")]
    public string? OutputFolder { get; set; } = ".magma/obj/generated";

    /// <summary>
    /// Root folder where generated TypeScript is emitted.
    /// </summary>
    [JsonPropertyName("typescriptOutput")]
    public string? TypescriptOutput { get; set; } = ".magma/obj/ts";

    /// <summary>
    /// Additional named outputs (key: logical name, value: relative path).
    /// </summary>
    [JsonPropertyName("otherOutputs")]
    public Dictionary<string, string>? OtherOutputs { get; set; }
}

/// <summary>
/// Common compiler options.
/// </summary>
public interface ICompilerOptions
{
    /// <summary>
    /// Entry points (files) to compile.
    /// </summary>
    IReadOnlyList<string> EntryPoints { get; }

    /// <summary>
    /// Output directory for compiled artifacts.
    /// </summary>
    string? OutDir { get; }
}

/// <summary>
/// Esbuild-specific options.
/// </summary>
public sealed class EsbuildOptions : ICompilerOptions
{
    /// <summary>
    /// Entry points (files) to bundle/compile.
    /// </summary>
    [JsonPropertyName("entryPoints")]
    public List<string> EntryPointsRaw { get; set; } = [];

    /// <summary>
    /// Read-only view of <see cref="EntryPointsRaw"/>.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<string> EntryPoints => EntryPointsRaw;

    /// <summary>
    /// Output directory for compiled bundles.
    /// </summary>
    [JsonPropertyName("outdir")]
    public string? OutDir { get; set; } = "dist/scripts";

    /// <summary>
    /// Whether to bundle dependencies.
    /// </summary>
    [JsonPropertyName("bundle")]
    public bool? Bundle { get; set; } = true;

    /// <summary>
    /// Whether to minify output.
    /// </summary>
    [JsonPropertyName("minify")]
    public bool? Minify { get; set; } = true;

    /// <summary>
    /// Target ECMAScript version (e.g., "es2022").
    /// </summary>
    [JsonPropertyName("target")]
    public string? Target { get; set; } = "es2022";

    /// <summary>
    /// Whether to emit source maps.
    /// </summary>
    [JsonPropertyName("sourcemap")]
    public bool? SourceMap { get; set; } = false;
}

/// <summary>
/// SWC-specific options.
/// </summary>
public sealed class SwcOptions : ICompilerOptions
{
    /// <summary>
    /// Entry points (files) to compile/transpile.
    /// </summary>
    [JsonPropertyName("entryPoints")]
    public List<string> EntryPointsRaw { get; set; } = [];

    /// <summary>
    /// Read-only view of <see cref="EntryPointsRaw"/>.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<string> EntryPoints => EntryPointsRaw;

    /// <summary>
    /// Output directory for transpiled scripts.
    /// </summary>
    [JsonPropertyName("outdir")]
    public string? OutDir { get; set; } = "dist/scripts";

    /// <summary>
    /// SWC JSC target (e.g., "es2022").
    /// </summary>
    [JsonPropertyName("jscTarget")]
    public string? JscTarget { get; set; } = "es2022";

    /// <summary>
    /// Whether to minify output.
    /// </summary>
    [JsonPropertyName("minify")]
    public bool? Minify { get; set; } = true;

    /// <summary>
    /// Whether to emit source maps.
    /// </summary>
    [JsonPropertyName("sourceMaps")]
    public bool? SourceMaps { get; set; } = false;

    /// <summary>
    /// Emit CommonJS modules instead of ESM.
    /// </summary>
    [JsonPropertyName("moduleCommonJs")]
    public bool? ModuleCommonJs { get; set; } = false;
}