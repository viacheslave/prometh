namespace Prometh;

internal record MetricDefinition(string Name, string Help, MetricType Type)
{
  public MetricDefinition(string name, string help, string type)
    : this(name, help, ParseType(type))
  {
  }

  private static MetricType ParseType(string type)
  {
    return type?.ToLower() switch
    {
      "gauge" => MetricType.Gauge,
      "counter" => MetricType.Counter,
      "histogram" => MetricType.Histogram,
      "summary" => MetricType.Summary,
      _ => MetricType.Untyped
    };
  }
}
