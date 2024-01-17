namespace Frank.DependenciesExplorer;

public static class EnumerableHelper
{
    public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] sources) => sources.SelectMany(x => x);
}