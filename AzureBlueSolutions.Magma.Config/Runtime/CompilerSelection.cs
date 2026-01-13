using AzureBlueSolutions.Magma.Config.Models;

namespace AzureBlueSolutions.Magma.Config.Runtime;

public static class CompilerSelection
{
    public static (MagmaCompilerKind kind, ICompilerOptions? options) Resolve(MagmaConfig config)
    {
        var options = config.GetActiveCompilerOptions();
        return (config.Compiler, options);
    }
}