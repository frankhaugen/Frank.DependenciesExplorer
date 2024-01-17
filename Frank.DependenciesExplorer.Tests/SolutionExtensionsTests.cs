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
    
    // [Fact]
    public void Test1()
    {
        var solutionHelper = new SolutionHelper(new FileInfo(@"..\..\..\..\Frank.DependenciesExplorer.sln"));
        var solution = solutionHelper.GetSolution();
        var solutionNode = solution.GetSolutionNode();
        _outputHelper.WriteLine(solutionNode.PrintHierarchy());
    }
}