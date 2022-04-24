namespace Prometh;

public class GaugeMetric : Metric
{
  public override MetricType Type => MetricType.Gauge;

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

  public class GaugeMetricData
  {
    public MetricLabels Labels { get; }

    public double? Value { get; }

    public string ValueRaw { get; }

    public GaugeMetricData(IReadOnlyDictionary<string, string> labels, double? value, string valueRaw)
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
