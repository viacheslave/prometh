namespace Prometh;

internal class MetricNameSpecification : IMetricSpecification
{
  private readonly string _name;

  public MetricNameSpecification(string name)
  {
    _name = name;
  }

  public bool Satisfies(Metric metric)
  {
    return string.Equals(metric.Name, _name, StringComparison.InvariantCultureIgnoreCase);
  }
}
