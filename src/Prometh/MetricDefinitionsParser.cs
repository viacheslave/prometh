using System.Text.RegularExpressions;

namespace Prometh;

internal static class MetricDefinitionsParser
{
  // https://prometheus.io/docs/concepts/data_model/#metric-names-and-labels

  private static readonly Regex _reType = new Regex("#\\sTYPE\\s(?<name>[a-zA-Z_:][a-zA-Z0-9_:]*)\\s(?<type>.*)");
  private static readonly Regex _reHelp = new Regex("#\\sHELP\\s(?<name>[a-zA-Z_:][a-zA-Z0-9_:]*)\\s(?<help>.*)");

  internal static IReadOnlyDictionary<string, MetricDefinition> Parse(string payload)
  {
    var lines = payload.Split('\n');

    var definitions = new List<MetricDefinition>();

    foreach (var line in lines)
    {
      var lineTrimmed = line.TrimEnd('\r');

      var matchType = _reType.Match(lineTrimmed);
      if (matchType.Success)
      {
        var name = matchType.Groups["name"].Value;
        var type = matchType.Groups["type"].Value;

        definitions.Add(new MetricDefinition(name, help: null, type: type));
        continue;
      }

      var matchHelp = _reHelp.Match(lineTrimmed);
      if (matchHelp.Success)
      {
        var name = matchHelp.Groups["name"].Value;
        var help = matchHelp.Groups["help"].Value;

        definitions.Add(new MetricDefinition(name, help: help, type: null));
      }
    }

    return definitions
      .GroupBy(definition => definition.Name)
      .Select(gr =>
      {
        var name = gr.Key;
        var help = gr.FirstOrDefault(item => !string.IsNullOrEmpty(item.Help))?.Help ?? default;
        var type = gr.FirstOrDefault(item => item.Type is not MetricType.Untyped)?.Type ?? MetricType.Untyped;

        return new MetricDefinition(Name: name, Help: help, Type: type);
      })
      .ToDictionary(definition => definition.Name);
  }
}
