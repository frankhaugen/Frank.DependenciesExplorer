# Frank.DependenciesExplorer

___
[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.1-4baaaa.svg)](https://www.contributor-covenant.org/version/2/1/code_of_conduct/code_of_conduct.md)
[![GitHub License](https://img.shields.io/github/license/frankhaugen/Frank.DependenciesExplorer)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Frank.DependenciesExplorer.svg)](https://www.nuget.org/packages/Frank.DependenciesExplorer)
[![NuGet](https://img.shields.io/nuget/dt/Frank.DependenciesExplorer.svg)](https://www.nuget.org/packages/Frank.DependenciesExplorer)

![GitHub contributors](https://img.shields.io/github/contributors/frankhaugen/Frank.DependenciesExplorer)
![GitHub Release Date - Published_At](https://img.shields.io/github/release-date/frankhaugen/Frank.DependenciesExplorer)
![GitHub last commit](https://img.shields.io/github/last-commit/frankhaugen/Frank.DependenciesExplorer)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/frankhaugen/Frank.DependenciesExplorer)
![GitHub pull requests](https://img.shields.io/github/issues-pr/frankhaugen/Frank.DependenciesExplorer)
![GitHub issues](https://img.shields.io/github/issues/frankhaugen/Frank.DependenciesExplorer)
![GitHub closed issues](https://img.shields.io/github/issues-closed/frankhaugen/Frank.DependenciesExplorer)
___

## Description

This is a simple library and tool to explore the dependencies of a .NET project. It is based on the [NuGet.ProjectModel](https://www.nuget.org/packages/NuGet.ProjectModel/) package among others.

## Purpose

The purpose of this library is to provide a simple way to explore the dependencies of a .NET project. Its main case is to get more familiar with the dependencies of a project through nested dependencies and transitive dependencies. A very common use case is to see how deep the dependencies go and what are the transitive dependencies.

## Usage

The library is available as a [NuGet package](https://www.nuget.org/packages/Frank.DependenciesExplorer/). It can be used as a library or as a tool.

### As a library

The library can be used as a library in any .NET project. It is available as a [NuGet package](https://www.nuget.org/packages/Frank.DependenciesExplorer/).

```csharp
using Frank.DependenciesExplorer;

public class Program
{
    private static string solutionFile = @"C:\path\to\solution.sln";
    
    public static void Main(string[] args)
    {
        var solution = DependenciesExplorer.GetSolution(solutionFile);
        var hierarchy = solution.GetSolutionNode().PrintHierarchy();
        Console.WriteLine(hierarchy);
    }
    
    // Output:
    // Solution
    //    Project1
    //      NuGetPackage1
    //      NuGetPackage2
    //        NuGetPackage3
    //    Project2
    //      NuGetPackage1
    //      NuGetPackage2
    //        NuGetPackage3
}
```

### As a tool

(This is not yet implemented)

## Contributing

Contributions are welcome. Please read the [contributing guidelines](CONTRIBUTING.md) first.

All contributions are subject to the [code of conduct](CODE_OF_CONDUCT.md).

## License

This project is licensed under the [MIT License](LICENSE).