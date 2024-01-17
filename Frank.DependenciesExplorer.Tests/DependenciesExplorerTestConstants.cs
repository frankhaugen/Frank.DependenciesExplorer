using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Build.Construction;

namespace Frank.DependenciesExplorer.Tests;

public static class DependenciesExplorerTestConstants
{
    public static readonly DirectoryInfo SolutionDirectory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "TestSolution"));
    public static readonly FileInfo SolutionFile = new FileInfo(Path.Combine(SolutionDirectory.FullName, "TestSolution.sln"));


    public static FileInfo CreatSolutionFile(Action<string> outputWriter)
    {
        // if (SolutionDirectory.Exists)
        // {
        //     // SolutionDirectory.Delete(true);
        // }
        //
        SolutionDirectory.Create();
        
        
        DotNetCliHelper.SetupSolution(SolutionFile, outputWriter);
        
        return SolutionFile;
    }
}

public static class DotNetCliHelper
{
    public static void SetupSolution(FileInfo solutionFile, Action<string> outputWriter, string projectName = "MyProject", string packageName = "Newtonsoft.Json")
    {
        // Create new solution
        CreateSolution(solutionFile, outputWriter);
        
        // Create a new project in the solution
        CreateProject(solutionFile, projectName, outputWriter);
        
        // Add a standard NuGet package to the newly created project
        AddPackage(new DirectoryInfo(Path.Combine(solutionFile.DirectoryName!, projectName)), projectName, packageName, outputWriter);
    }

    private static void CreateSolution(FileInfo solutionFile, Action<string> outputWriter)
    {
        Execute($"dotnet new sln -n {solutionFile.Name.Replace(".sln", "")} --force", outputWriter, solutionFile.Directory?.FullName ?? "");
    }

    private static void CreateProject(FileInfo solutionFile, string projectName, Action<string> outputWriter)
    {
        var projectDirectoryPath = Path.Combine(solutionFile.Directory?.FullName ?? "", projectName);
        var projectDirectory = new DirectoryInfo(projectDirectoryPath);
        var projectFile = new FileInfo(Path.Combine(projectDirectory.FullName, $"{projectName}.csproj"));

        outputWriter($"Creating project '{projectName}' in '{projectDirectoryPath}'.");
        
        projectDirectory.Create();
        
        Execute($"dotnet new console --force", outputWriter, projectDirectoryPath);
        Execute($"dotnet sln {solutionFile.FullName} add {projectFile.FullName}", outputWriter, projectDirectoryPath);
    }

    private static void AddPackage(DirectoryInfo projectDirectory, string projectName, string packageName, Action<string> outputWriter)
    {
        Execute($"dotnet add package {packageName}", outputWriter, projectDirectory.FullName);
    }
    
    private static void Execute(string command, Action<string> outputWriter, string workingDirectory)
    {
        var result = CliRunner.RunPowershellCommand(new DirectoryInfo(workingDirectory), command);

        if (result.StandardOutput != null)
        {
            outputWriter(result.StandardOutput);
        }
        
        if (result.StandardError != null)
        {
            outputWriter(result.StandardError);
        }
        
        if (result.ExitCode != 0)
        {
            // throw new Exception($"Command '{command}' failed with exit code {result.ExitCode}.");
        }
        
        // var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        //
        // var process = new Process()
        // {
        //     StartInfo = new ProcessStartInfo
        //     {
        //         FileName = "dotnet",
        //         // FileName = isWindows ? "cmd.exe" : "/bin/bash",
        //         Arguments = isWindows ? $"/c \"{command}\"" : $"-c \"{command}\"",
        //         RedirectStandardOutput = true,
        //         RedirectStandardError = true,
        //         CreateNoWindow = true,
        //         UseShellExecute = false,
        //         WorkingDirectory = workingDirectory
        //     }
        // };
        //
        // process.Start();
        //
        // outputWriter(process.StandardOutput.ReadToEnd());
        // outputWriter(process.StandardError.ReadToEnd());
        //
        // process.WaitForExit();
        //
        // if (process.ExitCode != 0)
        // {
        //     throw new Exception($"Command '{command}' failed with exit code {process.ExitCode}.");
        // }
    }
}