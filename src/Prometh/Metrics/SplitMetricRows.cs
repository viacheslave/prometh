namespace Prometh;

public class SplitMetricRows
{
  public IReadOnlyDictionary<double, double> Buckets { get; internal init; }

  public IReadOnlyList<(string key, string value)> BucketsRaw { get; internal init; }

  internal static SplitMetricRows Build(IEnumerable<MetricLine> lines, string splitKey)
  {
    var items = new HashSet<(double key, double value)>();
    var itemsRaw = new List<(string key, string value)>();

    foreach (var line in lines)
    {
      var key = line.Labels[splitKey];
      var value = line.Value;

      itemsRaw.Add((key, value));

      if (key.TryParse(out var q) && double.TryParse(value, out var v))
      {
        items.Add((q, v));
      }
    }

    return new SplitMetricRows
    {
      Buckets = new SortedDictionary<double, double>(
        items.ToDictionary(item => item.key, item => item.value)),

      BucketsRaw = itemsRaw
    };
  }

  public override int GetHashCode()
  {
    unchecked
    {
      var hash = 17;

      var bucketKeys = new SortedSet<double>(Buckets.Keys);
      foreach (var key in bucketKeys)
      {
        hash = hash * 23 + key.GetHashCode() + Buckets[key].GetHashCode();
      }

      foreach (var item in BucketsRaw)
      {
        hash = hash * 23 + item.GetHashCode();
      }

      return hash;
    }
  }

  public override bool Equals(object obj)
  {
    if (obj is null || obj is not SplitMetricRows other)
    {
      return false;
    }

    return EqualsBuckets(Buckets, other.Buckets) &&
      EqualsBuckets(BucketsRaw, other.BucketsRaw);
  }

  private bool EqualsBuckets(IReadOnlyDictionary<double, double> left, IReadOnlyDictionary<double, double> right)
  {
    return (left.Count == right.Count) && 
      left.All(l => 
        right.ContainsKey(l.Key) && 
        ((double.IsNaN(right[l.Key]) && double.IsNaN(l.Value)) || right[l.Key] == l.Value));
  }

  private bool EqualsBuckets(IReadOnlyList<(string key, string value)> left, IReadOnlyList<(string key, string value)> right)
  {
    return (left.Count == right.Count) && 
      left.All(l => right.Contains(l));
  }
}
