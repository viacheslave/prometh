namespace Prometh;

/// <summary>
///   Metric of type "gauge"
/// </summary>
public class GaugeMetric : Metric
{
  /// <inheritdoc/>
  public override MetricType Type => MetricType.Gauge;

  /// <summary>
  ///   Metric values by labels. <see cref="GaugeMetricData"/>
  /// </summary>
  public IReadOnlyCollection<GaugeMetricData> Lines { get; }

  internal GaugeMetric(string name, string help, IEnumerable<GaugeMetricData> lines)
    : base(name, help)
  {
    if (lines is null)
    {
      throw new ArgumentNullException(nameof(lines));
    }

    Lines = lines
      .Select(line => new GaugeMetricData(line?.Labels.Items, line?.Value, line?.ValueRaw))
      .ToList();
  }

  internal GaugeMetric(string name, string help, IEnumerable<MetricLine> lines)
    : base(name, help)
  {
    if (lines is null)
    {
      throw new ArgumentNullException(nameof(lines));
    }

    Lines = lines
      .Select(line => new GaugeMetricData(line))
      .ToList();
  }

  public override string ToString() => $"{Name} ({Lines.Count} line(s))";

  /// <summary>
  ///   Gauge metric data
  /// </summary>
  public class GaugeMetricData
  {
    /// <summary>
    ///   Metric labels. <see cref="MetricLabels"/>
    /// </summary>
    public MetricLabels Labels { get; }

    /// <summary>
    ///   Gauge value. Undefined if the parsing failed.
    /// </summary>
    public double? Value { get; }

    /// <summary>
    ///   Gauge raw text value.
    /// </summary>
    public string ValueRaw { get; }

    internal GaugeMetricData(IReadOnlyDictionary<string, string> labels, double? value, string valueRaw)
    {
      Labels = new MetricLabels(labels ?? new SortedDictionary<string, string>());

      ValueRaw = valueRaw;
      Value = value;
    }

    internal GaugeMetricData(MetricLine line)
    {
      if (line is null)
      {
        throw new ArgumentNullException(nameof(line));
      }

      Labels = new MetricLabels(line.Labels ?? new SortedDictionary<string, string>());

      ValueRaw = line.Value;

      if (line.Value.TryParse(out var value))
      {
        Value = value;
      }
    }

    public override string ToString() => $"value={Value} valueraw={ValueRaw} ({Labels})";
  }
}
