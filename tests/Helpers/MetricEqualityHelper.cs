namespace Prometh.Tests;

internal static class MetricEqualityHelper
{
  internal static bool EqualMetricLines(MetricLine left, MetricLine right)
  {
    return
      string.Equals(left.Value, right.Value) &&
      new MetricLineKey(left.Name, left.Labels).Equals(new MetricLineKey(right.Name, right.Labels));
  }

  internal static bool EqualMetrics(Metric left, Metric right)
  {
    return left.Type switch
    {
      MetricType.Untyped =>
        EqualMetricsSpecifics(left as UntypedMetric, right as UntypedMetric),
      MetricType.Counter =>
        EqualMetricsSpecific(left as CounterMetric, right as CounterMetric),
      MetricType.Gauge =>
        EqualMetricsSpecific(left as GaugeMetric, right as GaugeMetric),
      MetricType.Summary =>
        EqualMetricsSpecific(left as SummaryMetric, right as SummaryMetric),
      MetricType.Histogram =>
        EqualMetricsSpecific(left as HistogramMetric, right as HistogramMetric),
      _ => 
        throw new ArgumentOutOfRangeException(nameof(MetricType))
    };
  }

  private static bool EqualMetricsSpecific(HistogramMetric left, HistogramMetric right)
  {
    return 
      EqualBasicMetricAttributes(left, right) &&
      EqualSplitMetricData(left.Lines, right.Lines);
  }

  private static bool EqualMetricsSpecific(SummaryMetric left, SummaryMetric right)
  {
    return 
      EqualBasicMetricAttributes(left, right) &&
      EqualSplitMetricData(left.Lines, right.Lines);
  }

  private static bool EqualMetricsSpecific(GaugeMetric left, GaugeMetric right)
  {
    var result = EqualBasicMetricAttributes(left, right);

    var leftMap = left.Lines.ToDictionary(x => x.Labels);
    var rightMap = right.Lines.ToDictionary(x => x.Labels);

    foreach (var leftKvp in leftMap)
    {
      result &= rightMap.ContainsKey(leftKvp.Key);
      if (!result) return false;

      var lv = leftKvp.Value;
      var rv = rightMap[leftKvp.Key];

      result &= lv.Value.Equals(rv.Value);
      result &= string.Equals(lv.ValueRaw, rv.ValueRaw);
    }

    return result;
  }

  private static bool EqualMetricsSpecific(CounterMetric left, CounterMetric right)
  {
    var result = EqualBasicMetricAttributes(left, right);

    var leftMap = left.Lines.ToDictionary(x => x.Labels);
    var rightMap = right.Lines.ToDictionary(x => x.Labels);

    foreach (var leftKvp in leftMap)
    {
      result &= rightMap.ContainsKey(leftKvp.Key);
      if (!result) return false;

      var lv = leftKvp.Value;
      var rv = rightMap[leftKvp.Key];

      result &= lv.Value.Equals(rv.Value);
      result &= string.Equals(lv.ValueRaw, rv.ValueRaw);
    }

    return result;
  }

  private static bool EqualMetricsSpecifics(UntypedMetric left, UntypedMetric right)
  {
    var result = EqualBasicMetricAttributes(left, right);

    var leftMap = left.Lines.ToDictionary(x => x.Labels);
    var rightMap = right.Lines.ToDictionary(x => x.Labels);

    foreach (var leftKvp in leftMap)
    {
      result &= rightMap.ContainsKey(leftKvp.Key);
      if (!result) return false;

      var lv = leftKvp.Value;
      var rv = rightMap[leftKvp.Key];

      result &= lv.Value.Equals(rv.Value);
    }

    return result;
  }

  private static bool EqualBasicMetricAttributes(Metric left, Metric right)
  {
    return 
      left is not null &&
      right is not null &&
      string.Equals(left.Name, right.Name) &&
      string.Equals(left.Help, right.Help);
  }

  private static bool EqualSplitMetricData(IReadOnlyCollection<SplitMetricData> left, IReadOnlyCollection<SplitMetricData> right)
  {
    var result = left.Count == right.Count;

    var leftMap = left.ToDictionary(x => x.Labels);
    var rightMap = right.ToDictionary(x => x.Labels);

    foreach (var leftKvp in leftMap)
    {
      result &= rightMap.ContainsKey(leftKvp.Key);
      if (!result) return false;

      result &= leftKvp.Value.Equals(rightMap[leftKvp.Key]);
    }

    return result;
  }
}
