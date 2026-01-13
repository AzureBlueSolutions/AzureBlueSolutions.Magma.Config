namespace AzureBlueSolutions.Magma.Config.IO;

public static class PathResolver
{
    public static string GetAbsolutePath(string projectRoot, string relativeOrAbsolute)
    {
        if (string.IsNullOrWhiteSpace(relativeOrAbsolute)) return projectRoot;
        var p = relativeOrAbsolute.Replace('\\', '/');
        return Path.GetFullPath(Path.IsPathRooted(p)
            ? p
            : Path.Combine(projectRoot, p));
    }
}