namespace Prometh;

/// <summary>
///   Metric of type "histogram"
/// </summary>
public class HistogramMetric : Metric
{
  /// <inheritdoc/>
  public override MetricType Type => MetricType.Histogram;

  /// <inheritdoc/>
  public override IEnumerable<MetricLabels> Labels => Lines.Select(line => line.Labels);

  /// <summary>
  ///   Metric values by labels. <see cref="SplitMetricData"/>
  /// </summary>
  public IReadOnlyCollection<SplitMetricData> Lines { get; }

  internal HistogramMetric(
    string name,
    string help,
    IEnumerable<SplitMetricData> lines)
    : base(name, help)
  {
    _ = lines ?? throw new ArgumentNullException(nameof(lines));

    Lines = lines.ToList();
  }

  internal HistogramMetric(
    string name, 
    string help,
    IEnumerable<(IReadOnlyDictionary<string, string> labels, MetricLine lineSum, MetricLine lineCount, IEnumerable<MetricLine> linesBucket)> lines)
    : base(name, help)
  {
    _ = lines ?? throw new ArgumentNullException(nameof(lines));

    Lines = lines
      .Select(line => new SplitMetricData(line.labels, line.lineSum, line.lineCount, line.linesBucket, StringUtils.Le))
      .ToList();
  }

  public override string ToString() => $"{Name} ({Lines} line(s))";
}
