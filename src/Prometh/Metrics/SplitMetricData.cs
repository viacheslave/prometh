using System;

namespace Prometh;

/// <summary>
///   Histogram or Summary metric data
/// </summary>
public class SplitMetricData
{
  /// <summary>
  ///   Metric labels. <see cref="MetricLabels"/>
  /// </summary>
  public MetricLabels Labels { get; }

  /// <summary>
  ///   Sum value. Undefined if the parsing failed. 
  /// </summary>
  public double? Sum { get; }

  /// <summary>
  ///   Sum raw text value.
  /// </summary>
  public string SumRaw { get; }

  /// <summary>
  ///   Count value. Undefined if the parsing failed. 
  /// </summary>
  public long? Count { get; }

  /// <summary>
  ///   Count raw text value.
  /// </summary>
  public string CountRaw { get; }

  /// <summary>
  ///   Buckets data (le or quantiles). <see cref="SplitMetricRows"/>
  /// </summary>
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
