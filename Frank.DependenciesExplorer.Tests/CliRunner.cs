using System.Text;
using CliWrap;

namespace Frank.DependenciesExplorer.Tests;

public static class CliRunner
{
    public static CliResult RunPowershellCommand(DirectoryInfo directory, string command)
    {
        var standardOutputContainer = new StringBuilder();
        var errorOutputContainer = new StringBuilder();
    
        var shell = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "powershell" : "pwsh";
        
        var result = Cli.Wrap(shell)
            .WithArguments(command)
            .WithValidation(CommandResultValidation.None)
            .WithWorkingDirectory(directory.FullName)
            .WithStandardOutputPipe(PipeTarget.ToDelegate(x => standardOutputContainer.AppendLine(x)))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(x => errorOutputContainer.AppendLine(x)))
            .ExecuteAsync().GetAwaiter().GetResult();
    
        return new CliResult(
            result.ExitCode, 
            result.StartTime.DateTime, 
            result.ExitTime.DateTime, 
            standardOutputContainer.ToString(), 
            errorOutputContainer.ToString());
    }
}