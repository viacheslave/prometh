using Xunit;

namespace Prometh.Tests;

public class RawValueTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public RawValueTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void RawValueTests_GetRawValue_Gauge_ReturnsValue()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "kafka_consumer_lag";
    var labels = new Dictionary<string, string>
    {
      ["groupId"] = "sh2",
      ["topic"] = "mytopic",
      ["partition"] = "74"
    };

    var value = Metrics.GetRawValue(payload, name, labels);

    Assert.Equal("9", value);
  }

  [Fact]
  public void RawValueTests_GetRawValue_Counter_ReturnsValue()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "subroutine_counter";
    var labels = new Dictionary<string, string>
    {
      ["key"] = "shire",
    };

    var value = Metrics.GetRawValue(payload, name, labels);

    Assert.Equal("381524", value);
  }

  [Fact]
  public void RawValueTests_GetRawValue_Untyped_ReturnsValue()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "metrics_MilliSecondsSinceLastRecoveredChange";
    var labels = new Dictionary<string, string>
    {
      ["context"] = "schema-history",
      ["name"] = "decimal-02",
      ["plugin"] = "sql_server",
    };

    var value = Metrics.GetRawValue(payload, name, labels);

    Assert.Equal("9.1444414E7", value);
  }

  [Fact]
  public void RawValueTests_GetRawValue_Summary_Throws()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "subroutine_summary";
    var labels = new Dictionary<string, string>
    {
      ["key"] = "add",
    };

    Assert.Throws<ArgumentOutOfRangeException>(() =>
      Metrics.GetRawValue(payload, name, labels));
  }

  [Fact]
  public void RawValueTests_GetRawValue_Histogram_Throws()
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

    Assert.Throws<ArgumentOutOfRangeException>(() =>
      Metrics.GetRawValue(payload, name, labels));
  }

  [Fact]
  public void RawValueTests_GetRawValue_IncorrectName_Null()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var name = "incorrect";
    var labels = new Dictionary<string, string>
    {
    };

    var value = Metrics.GetRawValue(payload, name, labels);
    Assert.Null(value);
  }
}
