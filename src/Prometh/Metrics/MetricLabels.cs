using System.Text;

namespace Prometh;

public sealed class MetricLabels
{
  public IReadOnlyDictionary<string, string> Items { get; }

  public MetricLabels(IReadOnlyDictionary<string, string> items)
  {
    Items = items ?? throw new ArgumentNullException(nameof(items));
  }

  public override int GetHashCode()
  {
    unchecked
    {
      var hash = 17;

      var bucketKeys = new SortedSet<string>(Items.Keys);
      foreach (var key in bucketKeys)
      {
        hash = hash * 23 + key.GetHashCode() + Items[key].GetHashCode();
      }

      return hash;
    }
  }

  public override bool Equals(object obj)
  {
    if (obj is null || obj is not MetricLabels other)
    {
      return false;
    }

    return (Items.Count == other.Items.Count) &&
      Items.All(l => 
        other.Items.ContainsKey(l.Key) && 
        other.Items[l.Key] == l.Value);
  }

  public override string ToString()
  {
    var sb = new StringBuilder();

    foreach (var kvp in Items)
    {
      sb.Append($"{kvp.Key}={kvp.Value} ");
    }

    return sb.ToString();
  }
}
