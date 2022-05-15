using Xunit;

namespace Prometh.Tests;

public class ParserLargeOutputSpecificationNameLabelsTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public ParserLargeOutputSpecificationNameLabelsTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void MetricsTests_LargeOutput_ByNameAndLabels_Parse_Passes()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var labels = new Dictionary<string, string>
    {
      ["key"] = "add",
    };

    var metric = Metrics.Parse(payload, "subroutine_summary", labels);
    var expectedMetric = SubroutineSummaryMetric;

    Assert.NotNull(metric);

    Assert.True(
      MetricEqualityHelper.EqualMetrics(expectedMetric, metric));
  }

  [Fact]
  public void MetricsTests_LargeOutput_ByNameAndLabels_Parse_NotFound()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var labels = new Dictionary<string, string>
    {
      ["key"] = "remove",
    };

    var metric = Metrics.Parse(payload, "subroutine_summary", labels);

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
      });
}
