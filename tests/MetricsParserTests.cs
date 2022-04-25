using Xunit;

namespace Prometh.Tests;

public class MetricsParserTests : IClassFixture<AssemblyResourcesTestFixture>
{
  private readonly AssemblyResourcesTestFixture _resourcesTestFixture;

  public MetricsParserTests(AssemblyResourcesTestFixture resourcesTestFixture)
  {
    _resourcesTestFixture = resourcesTestFixture;
  }

  [Fact]
  public void MetricsTests_LargeOutput_Parse_Passes()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("large-output.data");

    var metrics = Metrics.Parse(payload)
      .ToDictionary(metric => metric.Name);

    var expectedMetrics = GetMetrics();

    Assert.Equal(expectedMetrics.Length, metrics.Count);

    foreach (var expectedMetric in expectedMetrics)
    {
      metrics.TryGetValue(expectedMetric.Name, out var metric);

      Assert.NotNull(metric);

      Assert.True(
        MetricEqualityHelper.EqualMetrics(expectedMetric, metric));
    }
  }

  [Fact]
  public void MetricsTests_GaugeDuplicate_Should_Return_SingleMetric()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("gauge-duplicates.data");

    var metrics = Metrics.Parse(payload);

    Assert.Single(metrics);

    var gaugeMetric = GetGaugeDuplicateMetric();

    Assert.NotNull(gaugeMetric);

    Assert.True(
      MetricEqualityHelper.EqualMetrics(gaugeMetric, metrics.First() as GaugeMetric));
  }

  [Fact]
  public void MetricsTests_SummaryDuplicate_Should_Return_SingleMetric()
  {
    var payload = _resourcesTestFixture.Resources.GetResourcePayload("summary-duplicates.data");

    var metrics = Metrics.Parse(payload);

    Assert.Single(metrics);

    var summaryMetric = GetSummaryDuplicateMetric();

    Assert.NotNull(summaryMetric);

    Assert.True(
      MetricEqualityHelper.EqualMetrics(summaryMetric, metrics.First() as SummaryMetric));
  }

  private static GaugeMetric GetGaugeDuplicateMetric()
  {
    var metric = new GaugeMetric(
      "kafka_consumer_lag",
      "Provides kafka consumer lag",
      new[]
      {
        new GaugeMetric.GaugeMetricData(
          new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "18", }, value: 4, valueRaw: "4"),
        new GaugeMetric.GaugeMetricData(
          new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "74", }, value: 9, valueRaw: "9"),
        new GaugeMetric.GaugeMetricData(
          new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "108", }, value: 6, valueRaw: "6"),
        new GaugeMetric.GaugeMetricData(
          new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "116", }, value: 5, valueRaw: "5"),
      });

    return metric;
  }

  private static SummaryMetric GetSummaryDuplicateMetric()
  {
    var metric = new SummaryMetric(
      "subroutine_summary",
      help: "The summary of subroutine processed",
      new[]
      {
        new SplitMetricData(
          labels: new Dictionary<string, string>() { ["key"] = "add" },
          sum: 8481939.800000006, sumRaw: "8481939.800000006",
          count: 4518, countRaw: "4518",
          new SplitMetricRows
          {
            Buckets = new Dictionary<double, double>
            {
              [0.5] = 391.5,
              [0.9] = 614.3,
              [0.95] = 691.6,
              [0.99] = 856.8,
            },
            BucketsRaw = new List<(string quantile, string value)>
            {
              ("0.5", "391.5"),
              ("0.9", "614.3"),
              ("0.95", "691.6"),
              ("0.99", "856.8"),
            }
          }
        ),
        new SplitMetricData(
          labels: new Dictionary<string, string>() { ["key"] = "update" },
          sum: 35907750.29999994, sumRaw: "35907750.29999994",
          count: 3833, countRaw: "3833",
          new SplitMetricRows
          {
            Buckets = new Dictionary<double, double>
            {
              [0.5] = 926.6,
              [0.9] = 1252.6999999999998,
              [0.95] = 1565,
              [0.99] = 406884.5,
            },
            BucketsRaw = new List<(string quantile, string value)>
            {
              ("0.5", "926.6"),
              ("0.9", "1252.6999999999998"),
              ("0.95", "1565"),
              ("0.99", "406884.5"),
            }
          }
        ),
        new SplitMetricData(
          labels: new Dictionary<string, string>() { ["key"] = "delete" },
          sum: 1554.7, sumRaw: "1554.7",
          count: 1, countRaw: "1",
          new SplitMetricRows
          {
            Buckets = new Dictionary<double, double>
            {
              [0.5] = double.NaN,
              [0.9] = double.NaN,
              [0.95] = double.NaN,
              [0.99] = double.NaN,
            },
            BucketsRaw = new List<(string quantile, string value)>
            {
              ("0.5", "NaN"),
              ("0.9", "NaN"),
              ("0.95", "NaN"),
              ("0.99", "NaN"),
            }
          }
        )
      });

    return metric;
  }

  private static Metric[] GetMetrics()
  {
    return new Metric[] 
    { 
      new GaugeMetric(
        "process_virtual_memory_bytes",
        "Virtual memory size in bytes.",
        new []
        {
          new GaugeMetric.GaugeMetricData(
            new Dictionary<string, string>(), value: 2251686019072, valueRaw: "2251686019072")
        }),

      new CounterMetric(
        "exceptions_total_counter",
        "Provides the total count of exceptions",
        new []
        {
          new CounterMetric.CounterMetricData(
            new Dictionary<string, string>(), value: 0, valueRaw: "0")
        }),

      new GaugeMetric(
        "process_private_memory_bytes",
        "Process private memory size",
        new []
        {
          new GaugeMetric.GaugeMetricData(
            new Dictionary<string, string>(), value: 1192886272, valueRaw: "1192886272")
        }),

      new CounterMetric(
        "subroutine_counter",
        "Provides the count of subroutine processed",
        new []
        {
          new CounterMetric.CounterMetricData(
            new Dictionary<string, string>() { ["key"] = "shire" }, value: 381524, valueRaw: "381524")
        }),

      new GaugeMetric(
        "kafka_consumer_lag",
        "Provides kafka consumer lag",
        new []
        {
          new GaugeMetric.GaugeMetricData(
            new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "18", }, value: 4, valueRaw: "4"),
          new GaugeMetric.GaugeMetricData(
            new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "74", }, value: 9, valueRaw: "9"),
          new GaugeMetric.GaugeMetricData(
            new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "108", }, value: 6, valueRaw: "6"),
          new GaugeMetric.GaugeMetricData(
            new Dictionary<string, string>() { ["groupId"] = "sh2", ["topic"] = "mytopic", ["partition"] = "116", }, value: 5, valueRaw: "5"),
        }),

      new SummaryMetric(
        "subroutine_summary",
        help: "The summary of subroutine processed",
        new []
        {
          new SplitMetricData(
            labels: new Dictionary<string, string>() { ["key"] = "add" },
            sum: 8481939.800000006, sumRaw: "8481939.800000006",
            count: 4518, countRaw: "4518",
            new SplitMetricRows
            {
              Buckets = new Dictionary<double, double>
              {
                [0.5] = 391.5,
                [0.9] = 614.3,
                [0.95] = 691.6,
                [0.99] = 856.8,
              },
              BucketsRaw = new List<(string quantile, string value)>
              {
                ("0.5", "391.5"),
                ("0.9", "614.3"),
                ("0.95", "691.6"),
                ("0.99", "856.8"),
              }
            }
          ),
          new SplitMetricData(
            labels: new Dictionary<string, string>() { ["key"] = "update" },
            sum: 35907750.29999994, sumRaw: "35907750.29999994",
            count: 3833, countRaw: "3833",
            new SplitMetricRows
            {
              Buckets = new Dictionary<double, double>
              {
                [0.5] = 926.6,
                [0.9] = 1252.6999999999998,
                [0.95] = 1565,
                [0.99] = 406884.5,
              },
              BucketsRaw = new List<(string quantile, string value)>
              {
                ("0.5", "926.6"),
                ("0.9", "1252.6999999999998"),
                ("0.95", "1565"),
                ("0.99", "406884.5"),
              }
            }
          ),
          new SplitMetricData(
            labels: new Dictionary<string, string>() { ["key"] = "delete" },
            sum: 1554.7, sumRaw: "1554.7",
            count: 1, countRaw: "1",
            new SplitMetricRows
            {
              Buckets = new Dictionary<double, double>
              {
                [0.5] = double.NaN,
                [0.9] = double.NaN,
                [0.95] = double.NaN,
                [0.99] = double.NaN,
              },
              BucketsRaw = new List<(string quantile, string value)>
              {
                ("0.5", "NaN"),
                ("0.9", "NaN"),
                ("0.95", "NaN"),
                ("0.99", "NaN"),
              }
            }
          )
        }),

      new HistogramMetric(
        "http_request_duration_seconds",
        "The duration of HTTP requests processed by an ASP.NET Core application.",
        new []
        {
          new SplitMetricData(
            labels: new Dictionary<string, string>() { ["code"] = "200", ["method"] = "GET", ["controller"] = "Stats", ["action"] = "All" },
            sum: 0.0410965, sumRaw: "0.0410965",
            count: 6, countRaw: "6",
            new SplitMetricRows
            {
              Buckets = new Dictionary<double, double>
              {
                [0.001] = 4,
                [0.002] = 5,
                [0.004] = 5,
                [0.008] = 5,
                [0.016] = 5,
                [0.032] = 5,
                [0.064] = 6,
                [0.128] = 6,
                [0.256] = 6,
                [0.512] = 6,
                [1.024] = 6,
                [2.048] = 6,
                [4.096] = 6,
                [8.192] = 6,
                [16.384] = 6,
                [32.768] = 6,
                [double.PositiveInfinity] = 6,
              },
              BucketsRaw = new List<(string quantile, string value)>
              {
                ("0.001", "4"),
                ("0.002", "5"),
                ("0.004", "5"),
                ("0.008", "5"),
                ("0.016", "5"),
                ("0.032", "5"),
                ("0.064", "6"),
                ("0.128", "6"),
                ("0.256", "6"),
                ("0.512", "6"),
                ("1.024", "6"),
                ("2.048", "6"),
                ("4.096", "6"),
                ("8.192", "6"),
                ("16.384", "6"),
                ("32.768", "6"),
                ("+Inf", "6"),
              }
            }
          ),
        }),

      new HistogramMetric(
        "jvm_gc_collection_seconds",
        "Time spent in a given JVM garbage collector in seconds.",
        new []
        {
          new SplitMetricData(
            labels: new Dictionary<string, string>() { ["gc"] = "G1 Young Generation" },
            sum: 2.029, sumRaw: "2.029",
            count: 122, countRaw: "122.0",
            new SplitMetricRows
            {
              Buckets = new Dictionary<double, double>(),
              BucketsRaw = new List<(string quantile, string value)>()
            }
          ),
          new SplitMetricData(
            labels: new Dictionary<string, string>() { ["gc"] = "G1 Old Generation" },
            sum: 0, sumRaw: "0.0",
            count: 0, countRaw: "0.0",
            new SplitMetricRows
            {
              Buckets = new Dictionary<double, double>(),
              BucketsRaw = new List<(string quantile, string value)>()
            }
          ),
        }),

      new GaugeMetric(
        "process_start_time_seconds",
        "Start time of the process since unix epoch in seconds.",
        new []
        {
          new GaugeMetric.GaugeMetricData(
            new Dictionary<string, string>(), value: 1.650547219064E9, valueRaw: "1.650547219064E9")
        }),

      new UntypedMetric(
        "metrics_MilliSecondsSinceLastRecoveredChange",
        "MilliSecondsSinceLastRecoveredChange (data.sql_server<type=connector-metrics, context=schema-history, server=decimal><>MilliSecondsSinceLastRecoveredChange)",
        new []
        {
          new UntypedMetric.UntypedMetricData(
            new Dictionary<string, string>() { ["context"] = "schema-history", ["name"] = "decimal-02", ["plugin"] = "sql_server" }, 
            value: "9.1444414E7"),
          new UntypedMetric.UntypedMetricData(
            new Dictionary<string, string>() { ["context"] = "schema-history", ["name"] = "decimal-03", ["plugin"] = "sql_server" },
            value: "9.1444495E7")
        }),
    };
  }
}
