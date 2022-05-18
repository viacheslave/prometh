using Xunit;

namespace Prometh.Tests;

public class GaugeValueTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public GaugeValueTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void GaugeValueTests_GetGaugeValue_ReturnsValue()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "kafka_consumer_lag";
    var labels = new Dictionary<string, string>
    {
      ["groupId"] = "sh2",
      ["topic"] = "mytopic",
      ["partition"] = "18",
    };

    var value = Metrics.GetGaugeValue(payload, name, labels);

    Assert.Equal(4, value);
  }

  [Fact]
  public void GaugeValueTests_GetGaugeValue_IncorectType_Throws()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "metrics_MilliSecondsSinceLastRecoveredChange";
    var labels = new Dictionary<string, string>
    {
    };

    Assert.Throws<MetricTypeMismatchException>(() => Metrics.GetGaugeValue(payload, name, labels));
  }

  [Fact]
  public void GaugeValueTests_GetGaugeValue_IncorectName_Null()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "incorrect";
    var labels = new Dictionary<string, string>
    {
    };

    var value = Metrics.GetGaugeValue(payload, name, labels);
    Assert.Null(value);
  }
}
