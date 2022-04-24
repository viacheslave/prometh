using System.Text.RegularExpressions;

namespace Prometh;

internal static class MetricLineParser
{
  private static readonly Regex _reLine = new Regex("(?<name>[a-zA-Z_:][a-zA-Z0-9_:]*)(?<labels>\\{(.*)\\})?\\s(?<value>.*)");

  internal static IReadOnlyCollection<MetricLine> ParseMetrics(string payload)
  {
    var metricLines = new List<MetricLine>();

    var lines = payload.Split('\n');

    foreach (var line in lines)
    {
      if (line.StartsWith("#"))
      {
        continue;
      }

      var lineTrimmed = line.TrimEnd('\r');

      var match = _reLine.Match(lineTrimmed);
      if (match.Success)
      {
        var name = match.Groups["name"].Value;
        var value = match.Groups["value"].Value;

        var labelsRaw = match.Groups["labels"].Value
          .Trim(new char[] { '{', '}' });

        var labels =
          labelsRaw
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(entry => entry.Contains('='))
            .Select(entry => 
            {
              var entryParts = entry.Split('=');
              return (key: entryParts[0], value: entryParts[1]);
            })
            .Where(kv => kv.value.Length >= 2)
            .Select(kv => (key: kv.key, value: kv.value.Trim('"')))
            // remove duplicate labels
            .GroupBy(entry => entry.key)
            .Select(gr => gr.First())
            .ToDictionary(entry => entry.key, entry => entry.value);

        var metricLine = new MetricLine(name, value, new SortedDictionary<string, string>(labels));

        metricLines.Add(metricLine);
      }
    }

    return metricLines;
  }
}
