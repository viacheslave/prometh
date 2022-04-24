namespace Prometh;

internal record MetricLine(string Name, string Value, SortedDictionary<string, string> Labels);
