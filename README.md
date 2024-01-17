# Frank.DependenciesExplorer

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

// ...
```