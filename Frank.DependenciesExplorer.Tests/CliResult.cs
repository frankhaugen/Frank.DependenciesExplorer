using CliWrap;

namespace Frank.DependenciesExplorer.Tests;

public class CliResult(int exitCode, DateTime startTime, DateTime exitTime, string? standardOutput = null, string? standardError = null) : CommandResult(exitCode, startTime, exitTime)
{
    public string? StandardOutput { get; } = standardOutput;

    public string? StandardError { get; } = standardError;
}