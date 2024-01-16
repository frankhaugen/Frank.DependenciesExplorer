using System.Diagnostics.CodeAnalysis;
using Buildalyzer;
using Buildalyzer.Construction;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using SlnParser;
using SlnParser.Contracts;

namespace Frank.DependenciesExplorer;

class MyClass
{
    
void Main()
{
    var solutionPath = "D:/seminerepos/Semine.sln";
    var solutionParser = new SolutionParser();
    var solution = solutionParser.Parse(solutionPath);

    var tree = new Nuget()
    {
        Name = solution.Name,
        Version = "SOLUTION"
    };
    
    try
    {
        Run(solution, tree);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message + "\n");
    }
    
    // tree.Dump();
}


private void Run(ISolution solution, Nuget tree)
{
    var projects = solution.Projects;
    foreach (var projectFile in projects.Cast<SlnParser.Contracts.SolutionFolder>().SelectMany(x => x.Projects).Cast<SlnParser.Contracts.SolutionProject>().Select(x => x.File))
    {
        var project = new AnalyzerManager().GetProject(projectFile.FullName);
        var projectNuget = new Nuget() {
            Name = project.ProjectFile.Name,
            Version = "PROJECT"
        };
        
        var dependencyLookup = GetPackageDependencies(project);
        var visited = new HashSet<NugetBase>();

        var deps = project.ProjectFile.PackageReferences;
        foreach (var element in deps)
        {
            PopulateTree(element, projectNuget, dependencyLookup, visited);
        }
        
        tree.DependentNugets.Add(projectNuget);
    }
}

private static void PopulateTree(IPackageReference packageReference, Nuget parentNuget, NugetLookup dependencyLookup, HashSet<NugetBase> visited)
{
    var nuget = new Nuget()
    {
        Name = packageReference.Name,
        Version = packageReference.Version
    };

    // Prevent processing the same package for the same parent
    if (!visited.Add(new NugetBase { Name = nuget.Name, Version = nuget.Version }))
        return;

    var childDependencies = dependencyLookup.GetValueOrDefault(nuget) ?? new List<NugetBase>();
    foreach (var dep in childDependencies)
    {
        PopulateTree(dep, nuget, dependencyLookup, visited);
    }

    parentNuget.DependentNugets.Add(nuget);
}

static void PopulateTree(NugetBase dep, Nuget parentNuget, NugetLookup lookup, HashSet<NugetBase> visited)
{
    var nuget = new Nuget()
    {
        Name = dep.Name,
        Version = dep.Version
    };

    if (!visited.Add(new NugetBase { Name = nuget.Name, Version = nuget.Version }))
        return;

    var childDependencies = lookup.GetValueOrDefault(dep) ?? new List<NugetBase>();
    foreach (var pack in childDependencies)
    {
        PopulateTree(pack, nuget, lookup, visited);
    }

    parentNuget.DependentNugets.Add(nuget);
}



private static NugetLookup GetPackageDependencies(IProjectAnalyzer project)
{
    var projectFileInfo = new FileInfo(project.ProjectFile.Path);
    var lockFile = new LockFileFormat().Read(Path.Combine(projectFileInfo.Directory?.FullName ?? "", "obj", "project.assets.json"));

    var libraries = lockFile.Targets
        .SelectMany(target => target.Libraries)
        .Where(library => library.Type == LibraryType.Package);

    var packages = new NugetLookup();

    foreach (var library in libraries)
    {
        var dependends = library.Dependencies.Select(dependency => new NugetBase { Name = dependency.Id, Version = dependency.VersionRange.ToNormalizedString() }).ToList();
        packages.Add(new NugetBase { Name = library.Name, Version = library.Version.ToNormalizedString() }, dependends);
    }

    return packages;
}

}

class NugetLookup : Dictionary<NugetBase, List<NugetBase>>
{
    public NugetLookup() : base(new Dictionary<NugetBase, List<NugetBase>>(), new NugetBaseComparer())
    {
    }
}

class NugetBaseComparer : IEqualityComparer<NugetBase>
{
    public bool Equals(NugetBase? x, NugetBase? y)
    {
        return x?.Equals(y) ?? y is null;
    }

    public int GetHashCode([DisallowNull] NugetBase obj)
    {
        return obj.GetHashCode();
    }
}
class NugetBase
{
    public string? Name { get; set; }
    public string? Version { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is NugetBase other &&
               Name == other.Name &&
               Version == other.Version;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Version);
    }
}

class Nuget : NugetBase
{
    public List<Nuget> DependentNugets { get; } = new();

    public override bool Equals(object? obj)
    {
        return obj is Nuget nuget &&
               base.Equals(obj) &&
               DependentNugets == nuget.DependentNugets;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), DependentNugets);
    }
}