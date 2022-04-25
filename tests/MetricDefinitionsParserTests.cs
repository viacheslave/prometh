using Xunit;

namespace Prometh.Tests;

public class MetricDefinitionsParserTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public MetricDefinitionsParserTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void MetricDefinitionsParser_LargeOutput_Parse_Passes()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var definitions = MetricDefinitionsParser.Parse(payload);
    var expectedDefinitions = GetDefinitions();

    Assert.Equal(expectedDefinitions.Length, definitions.Count);

    foreach (var expectedDefinition in expectedDefinitions)
    {
      definitions.TryGetValue(expectedDefinition.Name, out var item);

      Assert.NotNull(item);
      Assert.Equal(expectedDefinition, item);
    }
  }

  private static MetricDefinition[] GetDefinitions()
  {
    return new[]
    {
      new MetricDefinition("process_virtual_memory_bytes",
        "Virtual memory size in bytes.",
        MetricType.Gauge),

      new MetricDefinition("exceptions_total_counter",
        "Provides the total count of exceptions",
        MetricType.Counter),

      new MetricDefinition("process_private_memory_bytes",
        "Process private memory size",
        MetricType.Gauge),

      new MetricDefinition("exceptions_counter",
        "Provides the count of exceptions by source and kind",
        MetricType.Counter),

      new MetricDefinition("subroutine_counter",
        "Provides the count of subroutine processed",
        MetricType.Counter),

      new MetricDefinition("kafka_consumer_lag",
        "Provides kafka consumer lag",
        MetricType.Gauge),

      new MetricDefinition("subroutine_summary",
        "The summary of subroutine processed",
        MetricType.Summary),

      new MetricDefinition("http_request_duration_seconds",
        "The duration of HTTP requests processed by an ASP.NET Core application.",
        MetricType.Histogram),

      new MetricDefinition("jvm_gc_collection_seconds",
        "Time spent in a given JVM garbage collector in seconds.",
        MetricType.Summary),

      new MetricDefinition("process_start_time_seconds",
        "Start time of the process since unix epoch in seconds.",
        MetricType.Gauge),

      new MetricDefinition("metrics_MilliSecondsSinceLastRecoveredChange",
        "MilliSecondsSinceLastRecoveredChange (data.sql_server<type=connector-metrics, context=schema-history, server=decimal><>MilliSecondsSinceLastRecoveredChange)",
        MetricType.Untyped),
    };
  }
}
