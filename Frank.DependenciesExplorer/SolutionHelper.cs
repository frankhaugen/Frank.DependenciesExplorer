using Buildalyzer;
using SlnParser;
using SlnParser.Contracts;

namespace Frank.DependenciesExplorer;

public class SolutionHelper(FileSystemInfo solutionFile)
{
    private readonly SolutionParser _solutionParser = new();
    
    public ISolution GetSolution() => _solutionParser.Parse(solutionFile.FullName);
    
    public IEnumerable<IProjectAnalyzer> GetProjects() => GetSolution().GetProjects();

    public Nuget GetNugetTree()
    {
        var solution = GetSolution();
        var tree = solution.GetPackageDependencies();
        return tree;
    }
}