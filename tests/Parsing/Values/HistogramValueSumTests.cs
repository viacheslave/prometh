using Xunit;

namespace Prometh.Tests;

public class HistogramValueSumTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public HistogramValueSumTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void HistogramValueSumTests_GetHistogramValue_ReturnsValue()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "http_request_duration_seconds";
    var labels = new Dictionary<string, string>
    {
      ["code"] = "200",
      ["method"] = "GET",
      ["controller"] = "Stats",
      ["action"] = "All",
    };

    var value = Metrics.GetHistogramSum(payload, name, labels);

    Assert.Equal(0.0410965, value);
  }

  [Fact]
  public void HistogramValueSumTests_GetHistogramValue_IncorectType_Throws()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "metrics_MilliSecondsSinceLastRecoveredChange";
    var labels = new Dictionary<string, string>
    {
    };

    Assert.Throws<MetricTypeMismatchException>(() => Metrics.GetHistogramSum(payload, name, labels));
  }

  [Fact]
  public void HistogramValueSumTests_GetHistogramValue_IncorectName_Null()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "incorrect";
    var labels = new Dictionary<string, string>
    {
    };

    var value = Metrics.GetHistogramSum(payload, name, labels);
    Assert.Null(value);
  }
}
