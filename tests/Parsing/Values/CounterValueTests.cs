using Xunit;

namespace Prometh.Tests;

public class CounterValueTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public CounterValueTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void CounterValueTests_GetCounterValue_ReturnsValue()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "subroutine_counter";
    var labels = new Dictionary<string, string>
    {
      ["key"] = "shire",
    };

    var value = Metrics.GetCounterValue(payload, name, labels);

    Assert.Equal(381524, value);
  }

  [Fact]
  public void CounterValueTests_GetCounterValue_IncorectType_Throws()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "metrics_MilliSecondsSinceLastRecoveredChange";
    var labels = new Dictionary<string, string>
    {
    };

    Assert.Throws<MetricTypeMismatchException>(() => Metrics.GetCounterValue(payload, name, labels));
  }

  [Fact]
  public void CounterValueTests_GetCounterValue_IncorectName_Null()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "incorrect";
    var labels = new Dictionary<string, string>
    {
    };

    var value = Metrics.GetCounterValue(payload, name, labels);
    Assert.Null(value);
  }
}
