using Xunit;

namespace Prometh.Tests;

public class SummaryValueCountTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public SummaryValueCountTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void SummaryValueCountTests_GetSummaryValue_ReturnsValue()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "subroutine_summary";
    var labels = new Dictionary<string, string>
    {
      ["key"] = "add",
    };

    var value = Metrics.GetSummaryCount(payload, name, labels);

    Assert.Equal(4518, value);
  }

  [Fact]
  public void SummaryValueCountTests_GetSummaryValue_IncorectType_Throws()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "metrics_MilliSecondsSinceLastRecoveredChange";
    var labels = new Dictionary<string, string>
    {
    };

    Assert.Throws<MetricTypeMismatchException>(() => Metrics.GetSummaryCount(payload, name, labels));
  }

  [Fact]
  public void SummaryValueCountTests_GetSummaryValue_IncorectName_Null()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "incorrect";
    var labels = new Dictionary<string, string>
    {
    };

    var value = Metrics.GetSummaryCount(payload, name, labels);
    Assert.Null(value);
  }
}
