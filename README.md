# Prometh

This is a .NET library that parses [Prometheus](http://prometheus.io/) metrics output.

The library targets:
- .NET Standard 2.0
- .NET 5

## Usage

Text payload:

```
# HELP kafka_consumer_lag Provides kafka consumer lag
# TYPE kafka_consumer_lag gauge
kafka_consumer_lag{groupId="gr0",topic="mytopic",partition="18"} 4
```

Parse metrics:

```cs
var payload = "...";
var metrics = Prometh.Metrics.Parse(payload);
```

Get particular metric value:

```cs
var payload = "...";

var name = "kafka_consumer_lag";
var labels = new Dictionary<string, string>
{
  ["groupId"] = "gr0",
  ["topic"] = "mytopic",
  ["partition"] = "18",
};

var value = Prometh.Metrics.Parse(payload, name, labels);
// value is 4
```

## NuGet

The library is available as a [NuGet package](https://www.nuget.org/packages/Vizhion.Prometh).