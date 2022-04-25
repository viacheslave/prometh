using Xunit;

namespace Prometh.Tests;

public class MetricLinesParsingTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public MetricLinesParsingTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void MetricLineParserTests_LargeOutput_Parse_Passes()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var lines = MetricLineParser.ParseMetrics(payload)
      .ToDictionary(metric => new MetricLineKey(metric.Name, metric.Labels));

    var expectedLines = GetMetricLines();

    Assert.Equal(expectedLines.Length, lines.Count);

    foreach (var expectedLine in expectedLines)
    {
      lines.TryGetValue(new MetricLineKey(expectedLine.Name, expectedLine.Labels), out var item);

      Assert.NotNull(item);
      Assert.True(
        MetricEqualityHelper.EqualMetricLines(expectedLine, item));
    }
  }

  private static MetricLine[] GetMetricLines()
  {
    return new[]
    {
      new MetricLine("process_virtual_memory_bytes", "2251686019072", new SortedDictionary<string, string>()),

      new MetricLine("exceptions_total_counter", "0", new SortedDictionary<string, string>()),

      new MetricLine("process_private_memory_bytes", "1192886272", new SortedDictionary<string, string>()),

      new MetricLine("subroutine_counter", "381524", new SortedDictionary<string, string>() { ["key"] = "shire" }),

      new MetricLine("kafka_consumer_lag", "4", new SortedDictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "18", }),
      new MetricLine("kafka_consumer_lag", "9", new SortedDictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "74", }),
      new MetricLine("kafka_consumer_lag", "6", new SortedDictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "108" }),
      new MetricLine("kafka_consumer_lag", "5", new SortedDictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "116" }),

      new MetricLine("subroutine_summary_sum", "8481939.800000006", new SortedDictionary<string, string>() { ["key"] = "add" }),
      new MetricLine("subroutine_summary_count", "4518",            new SortedDictionary<string, string>() { ["key"] = "add" }),
      new MetricLine("subroutine_summary", "391.5",                 new SortedDictionary<string, string>() { ["key"] = "add", ["quantile"] = "0.5" }),
      new MetricLine("subroutine_summary", "614.3",                 new SortedDictionary<string, string>() { ["key"] = "add", ["quantile"] = "0.9" }),
      new MetricLine("subroutine_summary", "691.6",                 new SortedDictionary<string, string>() { ["key"] = "add", ["quantile"] = "0.95" }),
      new MetricLine("subroutine_summary", "856.8",                 new SortedDictionary<string, string>() { ["key"] = "add", ["quantile"] = "0.99" }),

      new MetricLine("subroutine_summary_sum", "35907750.29999994", new SortedDictionary<string, string>() { ["key"] = "update" }),
      new MetricLine("subroutine_summary_count", "3833",            new SortedDictionary<string, string>() { ["key"] = "update" }),
      new MetricLine("subroutine_summary", "926.6",                 new SortedDictionary<string, string>() { ["key"] = "update", ["quantile"] = "0.5" }),
      new MetricLine("subroutine_summary", "1252.6999999999998",    new SortedDictionary<string, string>() { ["key"] = "update", ["quantile"] = "0.9" }),
      new MetricLine("subroutine_summary", "1565",                  new SortedDictionary<string, string>() { ["key"] = "update", ["quantile"] = "0.95" }),
      new MetricLine("subroutine_summary", "406884.5",              new SortedDictionary<string, string>() { ["key"] = "update", ["quantile"] = "0.99" }),

      new MetricLine("subroutine_summary_sum", "1554.7",            new SortedDictionary<string, string>() { ["key"] = "delete" }),
      new MetricLine("subroutine_summary_count", "1",               new SortedDictionary<string, string>() { ["key"] = "delete" }),
      new MetricLine("subroutine_summary", "NaN",                   new SortedDictionary<string, string>() { ["key"] = "delete", ["quantile"] = "0.5" }),
      new MetricLine("subroutine_summary", "NaN",                   new SortedDictionary<string, string>() { ["key"] = "delete", ["quantile"] = "0.9" }),
      new MetricLine("subroutine_summary", "NaN",                   new SortedDictionary<string, string>() { ["key"] = "delete", ["quantile"] = "0.95" }),
      new MetricLine("subroutine_summary", "NaN",                   new SortedDictionary<string, string>() { ["key"] = "delete", ["quantile"] = "0.99" }),
    
      new MetricLine("http_request_duration_seconds_sum", "0.0410965",  new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All"}),
      new MetricLine("http_request_duration_seconds_count", "6",        new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All" }),
      new MetricLine("http_request_duration_seconds_bucket", "4",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "0.001" }),
      new MetricLine("http_request_duration_seconds_bucket", "5",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "0.002" }),
      new MetricLine("http_request_duration_seconds_bucket", "5",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "0.004" }),
      new MetricLine("http_request_duration_seconds_bucket", "5",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "0.008" }),
      new MetricLine("http_request_duration_seconds_bucket", "5",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "0.016" }),
      new MetricLine("http_request_duration_seconds_bucket", "5",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "0.032" }),
      new MetricLine("http_request_duration_seconds_bucket", "6",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "0.064" }),
      new MetricLine("http_request_duration_seconds_bucket", "6",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "0.128" }),
      new MetricLine("http_request_duration_seconds_bucket", "6",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "0.256" }),
      new MetricLine("http_request_duration_seconds_bucket", "6",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "0.512" }),
      new MetricLine("http_request_duration_seconds_bucket", "6",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "1.024" }),
      new MetricLine("http_request_duration_seconds_bucket", "6",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "2.048" }),
      new MetricLine("http_request_duration_seconds_bucket", "6",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "4.096" }),
      new MetricLine("http_request_duration_seconds_bucket", "6",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "8.192" }),
      new MetricLine("http_request_duration_seconds_bucket", "6",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "16.384" }),
      new MetricLine("http_request_duration_seconds_bucket", "6",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "32.768" }),
      new MetricLine("http_request_duration_seconds_bucket", "6",       new SortedDictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All", ["le"] = "+Inf" }),
    
      new MetricLine("jvm_gc_collection_seconds_count", "122.0",  new SortedDictionary<string, string>() { ["gc"] = "G1 Young Generation" }),
      new MetricLine("jvm_gc_collection_seconds_sum",   "2.029",  new SortedDictionary<string, string>() { ["gc"] = "G1 Young Generation" }),
      new MetricLine("jvm_gc_collection_seconds_count", "0.0",    new SortedDictionary<string, string>() { ["gc"] = "G1 Old Generation" }),
      new MetricLine("jvm_gc_collection_seconds_sum",   "0.0",    new SortedDictionary<string, string>() { ["gc"] = "G1 Old Generation" }),

      new MetricLine("process_start_time_seconds", "1.650547219064E9", new SortedDictionary<string, string>()),

      new MetricLine("metrics_MilliSecondsSinceLastRecoveredChange", "9.1444414E7", new SortedDictionary<string, string>(){ ["context"] = "schema-history", ["name"] = "decimal-02", ["plugin"] = "sql_server" }),
      new MetricLine("metrics_MilliSecondsSinceLastRecoveredChange", "9.1444495E7", new SortedDictionary<string, string>(){ ["context"] = "schema-history", ["name"] = "decimal-03", ["plugin"] = "sql_server" }),
    };
  }
}
