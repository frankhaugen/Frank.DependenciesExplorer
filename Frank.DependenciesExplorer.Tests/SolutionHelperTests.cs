using FluentAssertions;
using Xunit.Abstractions;

namespace Frank.DependenciesExplorer.Tests;

public class SolutionHelperTests
{
    private readonly ITestOutputHelper _outputHelper;

    public SolutionHelperTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void Test1()
    {
        var solutionHelper = new SolutionHelper(new FileInfo(@"D:\repos\Frank.DependenciesExplorer\Frank.DependenciesExplorer.sln"));
        var nugetTree = solutionHelper.GetNugetTree();
        _outputHelper.WriteLine(nugetTree.ToString());

        nugetTree.DependentNugets.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public void Test2()
    {
        var solutionHelper = new SolutionHelper(new FileInfo(@"D:\repos\Frank.DependenciesExplorer\Frank.DependenciesExplorer.sln"));
        var projects = solutionHelper.GetSolution().GetProjects();

        foreach (var project in projects)
        {
            _outputHelper.WriteLine(project.GetLockFile().GetParentChildLookup());
        }
        
    }
    
    [Fact]
    public void Test3()
    {
        var solutionHelper = new SolutionHelper(new FileInfo(@"D:\repos\Frank.DependenciesExplorer\Frank.DependenciesExplorer.sln"));
        var projects = solutionHelper.GetSolution().GetProjects();

        foreach (var project in projects)
        {
            _outputHelper.WriteLine(project.GetLockFile().GetHierarchy());
        }
        
    }
}