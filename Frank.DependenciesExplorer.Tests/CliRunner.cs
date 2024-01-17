using System.Text;
using CliWrap;

namespace Frank.DependenciesExplorer.Tests;

public static class CliRunner
{
    public static CliResult RunPowershellCommand(DirectoryInfo directory, string command)
    {
        var standardOutputContainer = new StringBuilder();
        var errorOutputContainer = new StringBuilder();
    
        var result = Cli.Wrap("powershell")
            .WithArguments(command)
            .WithValidation(CommandResultValidation.None)
            // .WithCredentials(new Credentials())
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