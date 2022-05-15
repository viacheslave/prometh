using Xunit;

namespace Prometh.Tests;

public class ParserSingleLineTests
{
  [Theory]
  [InlineData("kafka_consumer_lag{groupId=\"sh2\",topic=\"mytopic\",partition=\"116\"} 5")]
  // duplicate labels
  [InlineData("kafka_consumer_lag{groupId=\"sh2\",groupId=\"sh2\",topic=\"mytopic\",partition=\"116\"} 5")]
  public void MetricsTests_ParseSingle_Passes(string payload)
  {
    var metrics = Metrics.Parse(payload);

    Assert.Single(metrics);

    Assert.True(
      MetricEqualityHelper.EqualMetrics(GetGaugeMetric(), metrics.First() as UntypedMetric));
  }

  [Theory]
  // name contains a space
  [InlineData("kafka_consumer_lag {groupId=\"sh2\",topic=\"mytopic\",partition=\"116\"} 5")]
  // non-single space before the value
  [InlineData("kafka_consumer_lag{groupId=\"sh2\",topic=\"mytopic\",partition=\"116\"}  5")]
  public void MetricsTests_ParseSingle_Fails(string payload)
  {
    var metrics = Metrics.Parse(payload);

    Assert.Single(metrics);

    Assert.False(
      MetricEqualityHelper.EqualMetrics(GetGaugeMetric(), metrics.First() as UntypedMetric));
  }

  [Theory]
  // missing value
  [InlineData("kafka_consumer_lag{groupId=\"sh2\",topic=\"mytopic\",partition=\"116\"}")]
  // missing labels and value
  [InlineData("kafka_consumer_lag")]
  public void MetricsTests_Emtry_Output(string payload)
  {
    var metrics = Metrics.Parse(payload);

    Assert.Empty(metrics);
  }

  private static UntypedMetric GetGaugeMetric()
  {
    var metric = new UntypedMetric(
      "kafka_consumer_lag",
      help: null,
      new[]
      {
        new UntypedMetric.UntypedMetricData(
          new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "116", }, value: "5"),
      });

    return metric;
  }
}
