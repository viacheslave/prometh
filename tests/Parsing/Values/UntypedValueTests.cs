using Xunit;

namespace Prometh.Tests;

public class UntypedValueTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public UntypedValueTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void UntypedValueTests_GetUntypedValue_ReturnsValue()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "metrics_MilliSecondsSinceLastRecoveredChange";
    var labels = new Dictionary<string, string>
    {
      ["context"] = "schema-history",
      ["name"] = "decimal-02",
      ["plugin"] = "sql_server",
    };

    var value = Metrics.GetUntypedValue(payload, name, labels);

    Assert.Equal("9.1444414E7", value);
  }

  [Fact]
  public void UntypedValueTests_GetUntypedValue_IncorectType_Throws()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "subroutine_summary";
    var labels = new Dictionary<string, string>
    {
      ["key"] = "add"
    };

    Assert.Throws<MetricTypeMismatchException>(() => Metrics.GetUntypedValue(payload, name, labels));
  }

  [Fact]
  public void UntypedValueTests_GetUntypedValue_IncorectName_Null()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "incorrect";
    var labels = new Dictionary<string, string>
    {
    };

    var value = Metrics.GetUntypedValue(payload, name, labels);
    Assert.Null(value);
  }

  [Fact]
  public void UntypedValueTests_GetUntypedValue_Ambiguous_Throws()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "metrics_MilliSecondsSinceLastRecoveredChange";
    var labels = new Dictionary<string, string>
    {
    };

    Assert.Throws<AmbiguousMetricValueException>(() => Metrics.GetUntypedValue(payload, name, labels));
  }
}
