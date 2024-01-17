using SlnParser;
using SlnParser.Contracts;

namespace Frank.DependenciesExplorer;

public static class SolutionFactory
{
    private static readonly SolutionParser SolutionParser = new();

    public static ISolution GetSolution(FileSystemInfo solutionFile) 
        => SolutionParser.Parse(solutionFile.FullName);
}