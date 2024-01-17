using NuGet.Packaging.Core;
using NuGet.ProjectModel;

namespace Frank.DependenciesExplorer;

public static class LockFileExtensions
{
    public static IEnumerable<LockFileTargetLibrary> GetLibraries(this LockFile lockFile) => lockFile.Targets.SelectMany(target => target.Libraries);
    
    public static NugetLookup GetNugetLookup(this LockFile lockFile)
    {
        var libraries = lockFile.GetLibraries();
        var packages = new NugetLookup();

        foreach (var library in libraries)
        {
            packages.Add(
                $"{library.Name}+{library.Version}", 
                library.Dependencies.Select(dependency => dependency.ToNugetBase()).ToList()
            );
        }

        return packages;
    }
    
    public static Dictionary<LockFileTargetLibrary, List<PackageDependency>> GetLookup(this LockFile lockFile)
        => lockFile
            .GetLibraries()
            .ToDictionary(
                library => library, 
                library => library
                    .Dependencies
                    .ToList());

    public static Dictionary<string, List<string>> GetParentChildLookup(this LockFile lockFile)
    {
        var packages = new Dictionary<string, List<string>>();

        foreach (var library in lockFile.GetLibraries())
        {
            var key = $"{library.Name} ({library.Version?.OriginalVersion})";
            if (!packages.ContainsKey(key))
            {
                packages.Add(key, new List<string>());
            }

            foreach (var dependency in library.Dependencies)
            {
                var child = $"{dependency.Id}({dependency.VersionRange?.ToString()})";
                packages[key].Add(child);
            }
        }

        return packages;
    }
    
    
    public static IList<PackageNode> GetHierarchy(this LockFile lockFile) 
        => lockFile.GetLibraries().Select(library => CreateTree(library, GetLookup(lockFile))).ToList();

    private static PackageNode CreateTree(LockFileTargetLibrary library, Dictionary<LockFileTargetLibrary, List<PackageDependency>> lookup)
    {
        var children = new List<PackageNode>();

        if (lookup.TryGetValue(library, out var value))
        {
            children.AddRange(value.Select(child => lookup.Keys.FirstOrDefault(lib => lib.Name == child.Id && lib.Version?.OriginalVersion == child.VersionRange?.MinVersion?.OriginalVersion))
                .OfType<LockFileTargetLibrary>()
                .ToList()
                .Select(childLibrary => CreateTree(childLibrary, lookup)));
        }

        return new PackageNode(library, children);
    }
}