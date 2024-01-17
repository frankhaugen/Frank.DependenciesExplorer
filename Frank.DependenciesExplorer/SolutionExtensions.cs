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
    
    public static Nuget GetPackageDependencies(this ISolution solution)
    {
        var tree = new Nuget
        {
            Name = solution.Name,
            Version = "SOLUTION"
        };
        
        var projects = solution.GetProjects();
        
        foreach (var project in projects)
        {
            try
            {
                var projectNuget = new Nuget
                {
                    Name = project.ProjectFile.Name,
                    Version = "PROJECT"
                };

                var dependencyLookup = project.GetNugetLookup();

                var deps = project.ProjectFile.PackageReferences;
                foreach (var element in deps) element.PopulateTree(projectNuget, dependencyLookup);

                tree.DependentNugets.Add(projectNuget);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }

        return tree;
    }
    
    public static SolutionNode GetSolutionNode(this ISolution solution)
    {
        var projects = solution.GetProjects();
        var solutionNode = new SolutionNode(solution.Name, new List<ProjectNode>());
        
        foreach (var project in projects)
        {
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
        }

        return solutionNode;
    } 
}