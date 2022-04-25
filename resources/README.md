# Prometh

This is a .NET library that parses [Prometheus](http://prometheus.io/) metrics output.

## Usage

```cs
var payload = 
@"
# HELP kafka_consumer_lag Provides kafka consumer lag
# TYPE kafka_consumer_lag gauge
kafka_consumer_lag{groupId="gr0",topic="mytopic",partition="18"} 4
";

var metrics = Prometh.Metrics.Parse(payload);
```
