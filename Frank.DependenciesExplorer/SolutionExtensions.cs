using Buildalyzer;
using SlnParser.Contracts;

namespace Frank.DependenciesExplorer;

public static class SolutionExtensions
{
    public static IEnumerable<IProjectAnalyzer> GetProjects(this ISolution solution) =>
        EnumerableHelper.Concat(
            solution.Projects
                .Where(x => x is SolutionFolder)
                .Cast<SolutionFolder>()
                .SelectMany(x => x.Projects)
                .Where(x => x is SolutionProject)
                .Cast<SolutionProject>()
                .Select(x => x.File)
                .Select(projectFile => new AnalyzerManager().GetProject(projectFile.FullName))
                .ToList(),
            solution.Projects
                .Where(x => x is SolutionProject)
                .Cast<SolutionProject>()
                .Select(x => x.File)
                .Select(projectFile => new AnalyzerManager().GetProject(projectFile.FullName))
                .ToList());

    public static SolutionNode GetSolutionNode(this ISolution solution)
    {
        var projects = solution.GetProjects();
        var solutionNode = new SolutionNode(solution.Name, new List<ProjectNode>());

        foreach (var project in projects)
            try
            {
                var lockFile = project.GetLockFile();
                var hierarchy = lockFile.GetHierarchy();
                var projectNode = new ProjectNode(project.ProjectFile.Name, hierarchy);
                solutionNode.Projects.Add(projectNode);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }

        return solutionNode;
    }
}