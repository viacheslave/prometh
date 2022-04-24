namespace Prometh;

public class CounterMetric : Metric
{
  public override MetricType Type => MetricType.Counter;

  public IReadOnlyCollection<CounterMetricData> Lines { get; }

  internal CounterMetric(string name, string help, IEnumerable < CounterMetricData> lines)
    : base(name, help)
  {
    if (lines is null)
    {
      throw new ArgumentNullException(nameof(lines));
    }

    Lines = lines
      .Select(line => new CounterMetricData(line?.Labels.Items, line?.Value, line?.ValueRaw))
      .ToList();
  }

  internal CounterMetric(string name, string help, IEnumerable<MetricLine> lines)
    : base(name, help)
  {
    if (lines is null)
    {
      throw new ArgumentNullException(nameof(lines));
    }

    Lines = lines
      .Select(line => new CounterMetricData(line))
      .ToList();
  }

  public override string ToString() => $"{Name} ({Lines.Count} line(s))";

  public class CounterMetricData
  {
    public MetricLabels Labels { get; }

    public long? Value { get; }

    public string ValueRaw { get; }

    public CounterMetricData(IReadOnlyDictionary<string, string> labels, long? value, string valueRaw)
    {
      Labels = new MetricLabels(labels ?? new SortedDictionary<string, string>());

      ValueRaw = valueRaw;
      Value = value;
    }

    internal CounterMetricData(MetricLine line)
    {
      if (line is null)
      {
        throw new ArgumentNullException(nameof(line));
      }

      Labels = new MetricLabels(line.Labels ?? new SortedDictionary<string, string>());

      ValueRaw = line.Value;

      if (line.Value.TryParse(out var value))
      {
        try
        {
          Value = Convert.ToInt64(value);
        }
        catch (OverflowException)
        {
        }
      }
    }

    public override string ToString() => $"value={Value} valueraw={ValueRaw} ({Labels})";
  }
}
