namespace Prometh;

public static class Metrics
{
  public static IReadOnlyCollection<Metric> Parse(string payload)
  {
    if (payload is null)
    {
      return new List<Metric>();
    }

    var definitions = MetricDefinitionsParser.Parse(payload);

    // remove duplicates
    var lines = MetricLineParser.ParseMetrics(payload)
      .GroupBy(line => new MetricLineKey(line.Name, line.Labels))
      .ToDictionary(
        g => g.Key,
        g => g.First());

    var keys = new HashSet<MetricLineKey>(lines.Keys);

    var metrics = new List<Metric>();

    // parse summary and histogram metrics

    var linkedKeys = new List<MetricLineKey>();

    var sumSuffix = string.Empty.WithSum();
    var sumKeys = keys.Where(line => line.Name.EndsWith(sumSuffix));

    foreach (var sumKey in sumKeys)
    {
      var metricName = sumKey.Name.Substring(0, sumKey.Name.Length - sumSuffix.Length);
      var metricCountName = metricName.WithCount();

      var countKey = keys.FirstOrDefault(
        key => 
          key.Name == metricCountName && 
          key.LabelsKey.Equals(sumKey.LabelsKey));

      // no corresponsing metric_count line
      if (countKey is null)
      {
        continue;
      }

      // match with summary
      var quantileKeys = GetQuantileKeys(sumKey, countKey, metricName);
      if (quantileKeys?.Count > 0)
      {
        definitions.TryGetValue(metricName, out var definition);

        var ls = new[]
        {
          (
            labels: (IReadOnlyDictionary<string, string>)sumKey.Labels, 
            lineSum: lines[sumKey], 
            lineCount: lines[countKey], 
            linesBucket: quantileKeys.Select(k => lines[k])
          )
        };

        var summary = new SummaryMetric(
          name: metricName,
          help: definition.Help,
          lines: ls);

        metrics.Add(summary);

        linkedKeys.Add(sumKey);
        linkedKeys.Add(countKey);
        linkedKeys.AddRange(quantileKeys);

        continue;
      }

      // match with histogram
      var bucketKeys = GetBucketKeys(sumKey, countKey, metricName.WithBucket());
      if (bucketKeys is not null)
      {
        definitions.TryGetValue(metricName, out var definition);

        var ls = new[]
        {
          (
            labels: (IReadOnlyDictionary<string, string>)sumKey.Labels,
            lineSum: lines[sumKey],
            lineCount: lines[countKey],
            linesBucket: bucketKeys.Select(k => lines[k])
          )
        };

        var histogram = new HistogramMetric(
          name: metricName,
          help: definition.Help,
          lines: ls);

        metrics.Add(histogram);

        linkedKeys.Add(sumKey);
        linkedKeys.Add(countKey);
        linkedKeys.AddRange(bucketKeys);
      }
    }

    // remove processed lines
    keys.ExceptWith(linkedKeys);

    foreach (var key in keys)
    {
      var metricName = key.Name;

      definitions.TryGetValue(metricName, out var definition);

      var metricType = definition?.Type ?? MetricType.Untyped;
      var metricHelp = definition?.Help;

      var metric = metricType switch
      {
        MetricType.Gauge => 
          new GaugeMetric(metricName, help: metricHelp, new[] { lines[key] }),

        MetricType.Counter => 
          new CounterMetric(metricName, help: metricHelp, new[] { lines[key] }),

        _ => 
          (Metric)new UntypedMetric(metricName, help: metricHelp, new[] { lines[key] }),
      };

      metrics.Add(metric);
    }

    return metrics
      .GroupBy(metric => metric.Name)
      .Select(gr => 
      {
        var fst = gr.First();

        var name = gr.Key;
        var help = fst.Help;
        var type = fst.Type;

        return type switch
        {
          MetricType.Untyped =>
            (Metric)new UntypedMetric(name, help, gr.SelectMany(m => ((UntypedMetric)m).Lines)),

          MetricType.Counter =>
            new CounterMetric(name, help, gr.SelectMany(m => ((CounterMetric)m).Lines)),

          MetricType.Gauge =>
            new GaugeMetric(name, help, gr.SelectMany(m => ((GaugeMetric)m).Lines)),

          MetricType.Summary =>
            new SummaryMetric(name, help, gr.SelectMany(m => ((SummaryMetric)m).Lines)),

          MetricType.Histogram =>
            new HistogramMetric(name, help, gr.SelectMany(m => ((HistogramMetric)m).Lines)),

          _ => throw new ArgumentOutOfRangeException(nameof(MetricType))
        };
      })
      .ToList();

    IReadOnlyCollection<MetricLineKey> GetQuantileKeys(MetricLineKey sumKey, MetricLineKey countKey, 
      string metricName)
    {
      if (sumKey.HasQuantile || countKey.HasQuantile)
      {
        return null;
      }

      return keys.Where(
        key =>
          key.Name == metricName &&
          key.LabelsExceptQuantileKey.Equals(sumKey.LabelsKey) &&
          key.HasQuantile)
        .ToList();
    }

    IReadOnlyCollection<MetricLineKey> GetBucketKeys(MetricLineKey sumKey, MetricLineKey countKey,
      string metricName)
    {
      if (sumKey.HasLe || countKey.HasLe)
      {
        return null;
      }

      return keys.Where(
        key =>
          key.Name == metricName &&
          key.LabelsExceptLeKey.Equals(sumKey.LabelsKey) &&
          key.HasLe)
        .ToList();
    }
  }
}
