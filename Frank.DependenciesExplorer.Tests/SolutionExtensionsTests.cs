using FluentAssertions;
using Xunit.Abstractions;

namespace Frank.DependenciesExplorer.Tests;

public class SolutionExtensionsTests
{
    private readonly ITestOutputHelper _outputHelper;

    public SolutionExtensionsTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void Test1()
    {
        var solutionNode = DependenciesExplorer.GetSolutionNode(SolutionFactory.GetSolution(DependenciesExplorerTestConstants.CreatSolutionFile(_outputHelper.WriteLine)));
        solutionNode.Projects.First().NugetDependencies.Should().HaveCount(1);
        _outputHelper.WriteLine(solutionNode.Projects.First().PrintHierarchy());
    }
}