using Xunit;

namespace Prometh.Tests;

public class HistogramValueTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public HistogramValueTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void HistogramValueTests_GetHistogramValue_ReturnsValue()
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
    var bucket = 32.768;

    var value = Metrics.GetHistogramValue(payload, name, labels, bucket);

    Assert.Equal(6, value);
  }

  [Fact]
  public void HistogramValueTests_GetHistogramValue_IncorrectQuantile_ReturnsNull()
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
    var bucket = 42;

    var value = Metrics.GetHistogramValue(payload, name, labels, bucket);

    Assert.Null(value);
  }

  [Fact]
  public void HistogramValueTests_GetHistogramValue_IncorectType_Throws()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "metrics_MilliSecondsSinceLastRecoveredChange";
    var labels = new Dictionary<string, string>
    {
    };
    var bucket = 0.5;

    Assert.Throws<MetricTypeMismatchException>(() => Metrics.GetHistogramValue(payload, name, labels, bucket));
  }

  [Fact]
  public void HistogramValueTests_GetHistogramValue_IncorectName_Null()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "incorrect";
    var labels = new Dictionary<string, string>
    {
    };
    var bucket = 0.5;

    var value = Metrics.GetHistogramValue(payload, name, labels, bucket);
    Assert.Null(value);
  }
}
