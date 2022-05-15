namespace Prometh;

/// <summary>
///   Metric labels provider
/// </summary>
interface ILabels
{
  /// <summary>
  /// Metric  labels
  /// </summary>
  IEnumerable<MetricLabels> Labels { get; }
}
