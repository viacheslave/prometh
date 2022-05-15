namespace Prometh;

/// <summary>
///   Base metric definition
/// </summary>
public abstract class Metric : ILabels
{
  /// <summary>
  ///   Metric name
  /// </summary>
  public string Name { get; }

  /// <summary>
  ///   Metric description from the # HELP section
  /// </summary>
  public string Help { get; }

  /// <summary>
  ///   Metric type. <see cref="MetricType"/>
  ///   Defined by implementations.
  /// </summary>
  public abstract MetricType Type { get; }

  /// <inheritdoc />
  public abstract IEnumerable<MetricLabels> Labels { get; }

  protected internal Metric(string name, string help)
  {
    Name = name ?? throw new ArgumentNullException(nameof(name));
    Help = help;
  }
}
