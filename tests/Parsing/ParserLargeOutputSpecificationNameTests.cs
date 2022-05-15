using Xunit;

namespace Prometh.Tests;

public class ParserLargeOutputSpecificationNameTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public ParserLargeOutputSpecificationNameTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void MetricsTests_LargeOutput_ByName_Parse_Passes()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var metric = Metrics.Parse(payload, "subroutine_summary");
    var expectedMetric = SubroutineSummaryMetric;

    Assert.NotNull(metric);

    Assert.True(
      MetricEqualityHelper.EqualMetrics(expectedMetric, metric));
  }

  [Fact]
  public void MetricsTests_LargeOutput_ByName_Parse_NotFound()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var metric = Metrics.Parse(payload, "subroutine_summary_count");

    Assert.Null(metric);
  }

  private static Metric SubroutineSummaryMetric =>
    new SummaryMetric(
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
}
