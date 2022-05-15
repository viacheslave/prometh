namespace Prometh;

internal class MetricLabelsSpecification : IMetricSpecification
{
  private readonly IReadOnlyDictionary<string, string> _labels;

  public MetricLabelsSpecification(IReadOnlyDictionary<string, string> labels)
  {
    _labels = labels;
  }

  public bool Satisfies(Metric metric)
  {
    // all metric lines here are single
    var metricLabels = metric.Labels.Single();

    return _labels.All(
      label =>
        metricLabels.Items.ContainsKey(label.Key) &&
        metricLabels.Items[label.Key] == label.Value);
  }
}
