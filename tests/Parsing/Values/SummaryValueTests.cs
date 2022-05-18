using Xunit;

namespace Prometh.Tests;

public class SummaryValueTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public SummaryValueTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void SummaryValueTests_GetSummaryValue_ReturnsValue()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "subroutine_summary";
    var labels = new Dictionary<string, string>
    {
      ["key"] = "add",
    };
    var quantile = 0.5;

    var value = Metrics.GetSummaryValue(payload, name, labels, quantile);

    Assert.Equal(391.5, value);
  }

  [Fact]
  public void SummaryValueTests_GetSummaryValue_IncorrectQuantile_ReturnsNull()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "subroutine_summary";
    var labels = new Dictionary<string, string>
    {
      ["key"] = "add",
    };
    var quantile = 0.51;

    var value = Metrics.GetSummaryValue(payload, name, labels, quantile);

    Assert.Null(value);
  }

  [Fact]
  public void SummaryValueTests_GetSummaryValue_IncorectType_Throws()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "metrics_MilliSecondsSinceLastRecoveredChange";
    var labels = new Dictionary<string, string>
    {
    };
    var quantile = 0.5;

    Assert.Throws<MetricTypeMismatchException>(() => Metrics.GetSummaryValue(payload, name, labels, quantile));
  }

  [Fact]
  public void SummaryValueTests_GetSummaryValue_IncorectName_Null()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "incorrect";
    var labels = new Dictionary<string, string>
    {
    };
    var quantile = 0.5;

    var value = Metrics.GetSummaryValue(payload, name, labels, quantile);
    Assert.Null(value);
  }
}
