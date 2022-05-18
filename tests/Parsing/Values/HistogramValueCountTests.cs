using Xunit;

namespace Prometh.Tests;

public class HistogramValueCountTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public HistogramValueCountTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void HistogramValueCountTests_GetHistogramValue_ReturnsValue()
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

    var value = Metrics.GetHistogramCount(payload, name, labels);

    Assert.Equal(6, value);
  }

  [Fact]
  public void HistogramValueCountTests_GetHistogramValue_IncorectType_Throws()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "metrics_MilliSecondsSinceLastRecoveredChange";
    var labels = new Dictionary<string, string>
    {
    };

    Assert.Throws<MetricTypeMismatchException>(() => Metrics.GetHistogramCount(payload, name, labels));
  }

  [Fact]
  public void HistogramValueCountTests_GetHistogramValue_IncorectName_Null()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "incorrect";
    var labels = new Dictionary<string, string>
    {
    };

    var value = Metrics.GetHistogramCount(payload, name, labels);
    Assert.Null(value);
  }
}
