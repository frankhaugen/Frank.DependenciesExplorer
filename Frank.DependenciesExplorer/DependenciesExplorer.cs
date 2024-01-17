using SlnParser.Contracts;

namespace Frank.DependenciesExplorer;

public static class DependenciesExplorer
{
    public static SolutionNode GetSolutionNode(ISolution solution) => solution.GetSolutionNode();

    public static ProjectNode GetProjectNode(IProject project) => project.GetProjectNode();

    public static ISolution GetSolution(FileSystemInfo solutionFile) => SolutionFactory.GetSolution(solutionFile);
}