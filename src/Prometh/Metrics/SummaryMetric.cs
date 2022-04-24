namespace Prometh;

public class SummaryMetric : Metric
{
  public override MetricType Type => MetricType.Summary;

  public IReadOnlyCollection<SplitMetricData> Lines { get; }

  internal SummaryMetric(
    string name,
    string help,
    IEnumerable<SplitMetricData> lines)
    : base(name, help)
  {
    _ = lines ?? throw new ArgumentNullException(nameof(lines));

    Lines = lines.ToList();
  }

  internal SummaryMetric(
    string name,
    string help,
    IEnumerable<(IReadOnlyDictionary<string, string> labels, MetricLine lineSum, MetricLine lineCount, IEnumerable<MetricLine> linesBucket)> lines)
    : base(name, help)
  {
    _ = lines ?? throw new ArgumentNullException(nameof(lines));

    Lines = lines
      .Select(line => new SplitMetricData(line.labels, line.lineSum, line.lineCount, line.linesBucket, StringUtils.Quantile))
      .ToList();
  }

  public override string ToString() => $"{Name} ({Lines} line(s))";
}
