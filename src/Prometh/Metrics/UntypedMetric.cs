namespace Prometh;

public class UntypedMetric : Metric
{
  public override MetricType Type => MetricType.Untyped;

  public IReadOnlyCollection<UntypedMetricData> Lines { get; }

  internal UntypedMetric(string name, string help, IEnumerable<UntypedMetricData> lines)
    : base(name, help)
  {
    if (lines is null)
    {
      throw new ArgumentNullException(nameof(lines));
    }

    Lines = lines
      .Select(line => new UntypedMetricData(line?.Labels.Items, line?.Value))
      .ToList();
  }

  internal UntypedMetric(string name, string help, IEnumerable<MetricLine> lines)
    : base(name, help)
  {
    if (lines is null)
    {
      throw new ArgumentNullException(nameof(lines));
    }

    Lines = lines
      .Select(line => new UntypedMetricData(line))
      .ToList();
  }

  public override string ToString() => $"{Name} ({Lines.Count} line(s))";

  public class UntypedMetricData
  {
    public MetricLabels Labels { get; }

    public string Value { get; }

    public UntypedMetricData(IReadOnlyDictionary<string, string> labels, string value)
    {
      Labels = new MetricLabels(labels ?? new SortedDictionary<string, string>());
      Value = value;
    }

    internal UntypedMetricData(MetricLine line)
    {
      if (line is null)
      {
        throw new ArgumentNullException(nameof(line));
      }

      Labels = new MetricLabels(line.Labels ?? new SortedDictionary<string, string>());
      Value = line.Value;
    }

    public override string ToString() =>$"value={Value} ({Labels})";
  }
}
