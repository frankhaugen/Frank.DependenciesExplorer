using Xunit.Abstractions;

namespace Frank.DependenciesExplorer.Tests;

[TestSubject(typeof(DependenciesExplorer))]
public class DependenciesExplorerTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void GetSolutionNode_ShouldReturnCorrectSolutionNode()
    {
        // Arrange
        var solutionFile = DependenciesExplorerTestConstants.CreatSolutionFile(outputHelper.WriteLine);
        var solution = DependenciesExplorer.GetSolution(solutionFile);
        
        // Act
        var result = DependenciesExplorer.GetSolutionNode(solution);
        
        // Assert
        Assert.NotNull(result);
        outputHelper.WriteLine(result.PrintHierarchy());
    }

    [Fact]
    public void GetProjectNode_ShouldReturnCorrectProjectNode()
    {
        // Arrange
        var solutionFile = DependenciesExplorerTestConstants.CreatSolutionFile(outputHelper.WriteLine);
        var solution = DependenciesExplorer.GetSolution(solutionFile);
        var project = solution.Projects.First();
        
        // Act
        var result = DependenciesExplorer.GetProjectNode(project);
        
        // Assert
        outputHelper.WriteLine(result.PrintHierarchy());
        Assert.NotNull(result);
    }

    [Fact]
    public void GetSolution_ShouldReturnCorrectSolution()
    {
        // Arrange
        var solutionFile = DependenciesExplorerTestConstants.CreatSolutionFile(outputHelper.WriteLine);
        
        // Act
        var result = DependenciesExplorer.GetSolution(solutionFile);

        // Assert
        outputHelper.WriteLine(result.GetSolutionNode().PrintHierarchy());
        Assert.NotNull(result);
    }

    [Fact]
    public void GetProjectNode_WhenProjectIsNull_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => DependenciesExplorer.GetProjectNode(null));
    }
}