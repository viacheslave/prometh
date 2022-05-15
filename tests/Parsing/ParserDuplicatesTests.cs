using Xunit;

namespace Prometh.Tests;

public class ParserDuplicatesTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public ParserDuplicatesTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void MetricsTests_GaugeDuplicate_Should_Return_SingleMetric()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("gauge-duplicates.data");

    var metrics = Metrics.Parse(payload);

    Assert.Single(metrics);

    var gaugeMetric = GetGaugeDuplicateMetric();

    Assert.NotNull(gaugeMetric);

    Assert.True(
      MetricEqualityHelper.EqualMetrics(gaugeMetric, metrics.First() as GaugeMetric));
  }

  [Fact]
  public void MetricsTests_SummaryDuplicate_Should_Return_SingleMetric()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("summary-duplicates.data");

    var metrics = Metrics.Parse(payload);

    Assert.Single(metrics);

    var summaryMetric = GetSummaryDuplicateMetric();

    Assert.NotNull(summaryMetric);

    Assert.True(
      MetricEqualityHelper.EqualMetrics(summaryMetric, metrics.First() as SummaryMetric));
  }

  private static GaugeMetric GetGaugeDuplicateMetric()
  {
    var metric = new GaugeMetric(
      "kafka_consumer_lag",
      "Provides kafka consumer lag",
      new[]
      {
        new GaugeMetric.GaugeMetricData(
          new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "18", }, value: 4, valueRaw: "4"),
        new GaugeMetric.GaugeMetricData(
          new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "74", }, value: 9, valueRaw: "9"),
        new GaugeMetric.GaugeMetricData(
          new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "108", }, value: 6, valueRaw: "6"),
        new GaugeMetric.GaugeMetricData(
          new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "116", }, value: 5, valueRaw: "5"),
      });

    return metric;
  }

  private static SummaryMetric GetSummaryDuplicateMetric()
  {
    var metric = new SummaryMetric(
      "subroutine_summary",
      help: "The summary of subroutine processed",
      new[]
      {
        new SplitMetricData(
          labels: new Dictionary<string, string>() { ["key"] = "add" },
          sum: 8481939.800000006, sumRaw: "8481939.800000006",
          count: 4518, countRaw: "4518",
          new SplitMetricRows
          {
            Buckets = new Dictionary<double, double>
            {
              [0.5] = 391.5,
              [0.9] = 614.3,
              [0.95] = 691.6,
              [0.99] = 856.8,
            },
            BucketsRaw = new List<(string quantile, string value)>
            {
              ("0.5", "391.5"),
              ("0.9", "614.3"),
              ("0.95", "691.6"),
              ("0.99", "856.8"),
            }
          }
        ),
        new SplitMetricData(
          labels: new Dictionary<string, string>() { ["key"] = "update" },
          sum: 35907750.29999994, sumRaw: "35907750.29999994",
          count: 3833, countRaw: "3833",
          new SplitMetricRows
          {
            Buckets = new Dictionary<double, double>
            {
              [0.5] = 926.6,
              [0.9] = 1252.6999999999998,
              [0.95] = 1565,
              [0.99] = 406884.5,
            },
            BucketsRaw = new List<(string quantile, string value)>
            {
              ("0.5", "926.6"),
              ("0.9", "1252.6999999999998"),
              ("0.95", "1565"),
              ("0.99", "406884.5"),
            }
          }
        ),
        new SplitMetricData(
          labels: new Dictionary<string, string>() { ["key"] = "delete" },
          sum: 1554.7, sumRaw: "1554.7",
          count: 1, countRaw: "1",
          new SplitMetricRows
          {
            Buckets = new Dictionary<double, double>
            {
              [0.5] = double.NaN,
              [0.9] = double.NaN,
              [0.95] = double.NaN,
              [0.99] = double.NaN,
            },
            BucketsRaw = new List<(string quantile, string value)>
            {
              ("0.5", "NaN"),
              ("0.9", "NaN"),
              ("0.95", "NaN"),
              ("0.99", "NaN"),
            }
          }
        )
      });

    return metric;
  }
}
