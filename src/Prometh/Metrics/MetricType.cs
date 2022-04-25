namespace Prometh;

/// <summary>
///   Metric type enumeration
/// </summary>
public enum MetricType
{
  /// <summary>
  ///   "# TYPE metric untyped"
  /// </summary>
  Untyped,

  /// <summary>
  ///   "# TYPE metric counter"
  /// </summary>
  Counter,

  /// <summary>
  ///   "# TYPE metric gauge"
  /// </summary>
  Gauge,

  /// <summary>
  ///   "# TYPE metric summary"
  /// </summary>
  Summary,

  /// <summary>
  ///   "# TYPE metric histogram"
  /// </summary>
  Histogram,
}
