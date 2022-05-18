namespace Prometh;

/// <summary>
///   Prometheus output parser
/// </summary>
public static class Metrics
{
  /// <summary>
  ///   Parses out metrics data
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <returns>A collection of metrics <see cref="Metric"/></returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  public static IReadOnlyCollection<Metric> Parse(string payload)
  {
    var metrics = MetricsBuilder.BuildMetrics(payload);

    return MetricsBuilder.GetGrouped(metrics);
  }

  /// <summary>
  ///   Parses out metrics data for specific metric name (case-insensitive)
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="name">Metric name</param>
  /// <returns>Metric instance <see cref="Metric"/></returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  public static Metric Parse(string payload, string name)
  {
    var specifications = new IMetricSpecification[]
    {
      new MetricNameSpecification(name)
    };

    var metrics = MetricsBuilder.BuildMetrics(payload)
      .Where(metric => specifications.All(specification => specification.Satisfies(metric)));

    return MetricsBuilder.GetGrouped(metrics)
      .FirstOrDefault();
  }

  /// <summary>
  ///   Parses out metrics data filtered by labels and values
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="labels">Labels filter</param>
  /// <returns>A collection of metrics <see cref="Metric"/></returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  public static IReadOnlyCollection<Metric> Parse(string payload, IReadOnlyDictionary<string, string> labels)
  {
    labels ??= new Dictionary<string, string>();

    var specifications = new IMetricSpecification[]
    {
      new MetricLabelsSpecification(labels)
    };

    var metrics = MetricsBuilder.BuildMetrics(payload)
      .Where(metric => specifications.All(specification => specification.Satisfies(metric)));

    return MetricsBuilder.GetGrouped(metrics);
  }

  /// <summary>
  ///   Parses out metrics data for specific metric name (case-insensitive) and filtered by labels and values
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="name">Metric name</param>
  /// <param name="labels">Labels filter</param>
  /// <returns>Metric instance <see cref="Metric"/></returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  public static Metric Parse(string payload, string name, IReadOnlyDictionary<string, string> labels)
  {
    labels ??= new Dictionary<string, string>();

    var specifications = new IMetricSpecification[]
    {
      new MetricNameSpecification(name),
      new MetricLabelsSpecification(labels)
    };

    var metrics = MetricsBuilder.BuildMetrics(payload)
      .Where(metric => specifications.All(specification => specification.Satisfies(metric)));

    return MetricsBuilder.GetGrouped(metrics)
      .FirstOrDefault();
  }

  /// <summary>
  ///   Gets metric's raw unparsed value.
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="name">Metric name</param>
  /// <param name="labels">Metric labels</param>
  /// <returns>Metric value</returns>
  public static string GetRawValue(string payload, string name, IReadOnlyDictionary<string, string> labels)
  {
    var metric = GetSingleLabeledMetric<Metric>(payload, name, labels);
    if (metric is null)
    {
      return null;
    }

    return MetricsBuilder.GetRawValue(metric);
  }

  /// <summary>
  ///   Gets untyped metric's value.
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="name">Metric name</param>
  /// <param name="labels">Metric labels</param>
  /// <returns>Metric value</returns>
  public static string GetUntypedValue(string payload, string name, IReadOnlyDictionary<string, string> labels)
  {
    var metric = GetSingleLabeledMetric<UntypedMetric>(payload, name, labels);
    if (metric is null)
    {
      return null;
    }

    return metric.Lines.First().Value; 
  }

  /// <summary>
  ///   Gets counter metric's value.
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="name">Metric name</param>
  /// <param name="labels">Metric labels</param>
  /// <returns>Metric value</returns>
  public static long? GetCounterValue(string payload, string name, IReadOnlyDictionary<string, string> labels)
  {
    var metric = GetSingleLabeledMetric<CounterMetric>(payload, name, labels);
    if (metric is null)
    {
      return null;
    }

    return metric.Lines.First().Value;
  }

  /// <summary>
  ///   Gets gauge metric's value.
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="name">Metric name</param>
  /// <param name="labels">Metric labels</param>
  /// <returns>Metric value</returns>
  public static double? GetGaugeValue(string payload, string name, IReadOnlyDictionary<string, string> labels)
  {
    var metric = GetSingleLabeledMetric<GaugeMetric>(payload, name, labels);
    if (metric is null)
    {
      return null;
    }

    return metric.Lines.First().Value;
  }

  /// <summary>
  ///   Gets summary metric's value by quantile.
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="name">Metric name</param>
  /// <param name="labels">Metric labels</param>
  /// <param name="quantile">Metric quantile</param>
  /// <returns>Metric value</returns>
  public static double? GetSummaryValue(string payload, string name, IReadOnlyDictionary<string, string> labels, double quantile)
  {
    var metric = GetSingleLabeledMetric<SummaryMetric>(payload, name, labels);
    if (metric is null)
    {
      return null;
    }

    var quantileRows = metric.Lines.First().Rows.Buckets;
    return quantileRows.TryGetValue(quantile, out var value) ? value : null;
  }

  /// <summary>
  ///   Gets summary metric's sum value.
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="name">Metric name</param>
  /// <param name="labels">Metric labels</param>
  /// <returns>Metric value</returns>
  public static double? GetSummarySum(string payload, string name, IReadOnlyDictionary<string, string> labels)
  {
    var metric = GetSingleLabeledMetric<SummaryMetric>(payload, name, labels);
    if (metric is null)
    {
      return null;
    }

    return metric.Lines.First().Sum;
  }

  /// <summary>
  ///   Gets summary metric's count value.
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="name">Metric name</param>
  /// <param name="labels">Metric labels</param>
  /// <returns>Metric value</returns>
  public static long? GetSummaryCount(string payload, string name, IReadOnlyDictionary<string, string> labels)
  {
    var metric = GetSingleLabeledMetric<SummaryMetric>(payload, name, labels);
    if (metric is null)
    {
      return null;
    }

    return metric.Lines.First().Count;
  }

  /// <summary>
  ///   Gets histogram metric's value by bucket.
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="name">Metric name</param>
  /// <param name="labels">Metric labels</param>
  /// <param name="bucket">Metric bucket</param>
  /// <returns>Metric value</returns>
  public static double? GetHistogramValue(string payload, string name, IReadOnlyDictionary<string, string> labels, double bucket)
  {
    var metric = GetSingleLabeledMetric<HistogramMetric>(payload, name, labels);
    if (metric is null)
    {
      return null;
    }

    var quantileRows = metric.Lines.First().Rows.Buckets;
    return quantileRows.TryGetValue(bucket, out var value) ? value : null;
  }

  /// <summary>
  ///   Gets histogram metric's sum value.
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="name">Metric name</param>
  /// <returns>Metric value</returns>
  public static double? GetHistogramSum(string payload, string name, IReadOnlyDictionary<string, string> labels)
  {
    var metric = GetSingleLabeledMetric<HistogramMetric>(payload, name, labels);
    if (metric is null)
    {
      return null;
    }

    return metric.Lines.First().Sum;
  }

  /// <summary>
  ///   Gets histogram metric's count value.
  /// </summary>
  /// <param name="payload">Text payload</param>
  /// <param name="name">Metric name</param>
  /// <returns>Metric value</returns>
  public static long? GetHistogramCount(string payload, string name, IReadOnlyDictionary<string, string> labels)
  {
    var metric = GetSingleLabeledMetric<HistogramMetric>(payload, name, labels);
    if (metric is null)
    {
      return null;
    }

    return metric.Lines.First().Count;
  }

  private static T GetSingleLabeledMetric<T>(string payload, string name, IReadOnlyDictionary<string, string> labels)
    where T: Metric
  {
    var metric = Parse(payload, name, labels);

    if (metric is null)
    {
      return null;
    }

    if (metric is not T)
    {
      throw new MetricTypeMismatchException(metric);
    }

    if (metric.Labels.Count() > 1)
    {
      throw new AmbiguousMetricValueException(metric);
    }

    return (T)metric;
  }
}
