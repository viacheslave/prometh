using Xunit;

namespace Prometh.Tests;

public class SummaryValueSumTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public SummaryValueSumTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void SummaryValueSumTests_GetSummaryValue_ReturnsValue()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "subroutine_summary";
    var labels = new Dictionary<string, string>
    {
      ["key"] = "add",
    };

    var value = Metrics.GetSummarySum(payload, name, labels);

    Assert.Equal(8481939.800000006, value);
  }

  [Fact]
  public void SummaryValueSumTests_GetSummaryValue_IncorectType_Throws()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "metrics_MilliSecondsSinceLastRecoveredChange";
    var labels = new Dictionary<string, string>
    {
    };

    Assert.Throws<MetricTypeMismatchException>(() => Metrics.GetSummarySum(payload, name, labels));
  }

  [Fact]
  public void SummaryValueSumTests_GetSummaryValue_IncorectName_Null()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "incorrect";
    var labels = new Dictionary<string, string>
    {
    };

    var value = Metrics.GetSummarySum(payload, name, labels);
    Assert.Null(value);
  }
}
