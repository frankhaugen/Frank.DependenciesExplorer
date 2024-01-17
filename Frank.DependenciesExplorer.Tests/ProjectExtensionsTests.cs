using Xunit.Abstractions;

namespace Frank.DependenciesExplorer.Tests;

public class ProjectExtensionsTests
{
    private readonly ITestOutputHelper _outputHelper;

    public ProjectExtensionsTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void Test1()
    {
        var solutionHelper = new SolutionHelper(new FileInfo(@"D:\repos\Frank.DependenciesExplorer\Frank.DependenciesExplorer.sln"));
        var solution = solutionHelper.GetSolution();
        var projects = solution.GetProjects();

        foreach (var project in projects)
        {
            var lookup = project.GetNugetLookup();
            _outputHelper.WriteLine(lookup);
        }
    }
}