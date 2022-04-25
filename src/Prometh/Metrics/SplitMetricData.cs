using System;

namespace Prometh;

public class SplitMetricData
{
  public MetricLabels Labels { get; }

  public double? Sum { get; }

  public string SumRaw { get; }

  public long? Count { get; }

  public string CountRaw { get; }

  public SplitMetricRows Rows { get; }

  internal SplitMetricData(IReadOnlyDictionary<string, string> labels, double? sum, string sumRaw, long? count, string countRaw,
    SplitMetricRows rows)
  {
    Labels = new MetricLabels(labels ?? throw new ArgumentNullException(nameof(labels)));
    Rows = rows ?? throw new ArgumentNullException(nameof(rows));

    Sum = sum;
    SumRaw = sumRaw;

    Count = count;
    CountRaw = countRaw;
  }

  internal SplitMetricData(IReadOnlyDictionary<string, string> labels, MetricLine lineSum, MetricLine lineCount,
    IEnumerable<MetricLine> lines, string splitKey)
  {
    Labels = new MetricLabels(labels ?? throw new ArgumentNullException(nameof(labels)));

    _ = lineSum ?? throw new ArgumentNullException(nameof(lineSum));
    _ = lineCount ?? throw new ArgumentNullException(nameof(lineCount));

    Rows = SplitMetricRows.Build(lines, splitKey);

    SumRaw = lineSum.Value;
    CountRaw = lineCount.Value;

    if (double.TryParse(SumRaw?.ToString(), out var sumValue))
    {
      Sum = sumValue;
    }

    if (double.TryParse(CountRaw?.ToString(), out var countValue))
    {
      try
      {
        Count = Convert.ToInt64(countValue);
      }
      catch (OverflowException)
      {
      }
    }
  }

  public override int GetHashCode() => HashCode.Combine(Labels, Rows, Sum, SumRaw, Count, CountRaw);

  public override bool Equals(object obj)
  {
    if (obj is null || obj is not SplitMetricData other)
    {
      return false;
    }

    return
      Sum.GetValueOrDefault().Equals(other.Sum.GetValueOrDefault()) &&
      Count.GetValueOrDefault().Equals(other.Count.GetValueOrDefault()) &&
      string.Equals(SumRaw, other.SumRaw) &&
      string.Equals(CountRaw, other.CountRaw) &&
      Labels.Equals(other.Labels) &&
      Rows.Equals(other.Rows);
  }

  public override string ToString() => 
    $"sum={Sum}, count={Count} ({Rows.Buckets.Count} buckets(s)) ({Labels})";
}
