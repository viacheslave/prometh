namespace Prometh;

internal class MetricLineLabelsKey
{
  public SortedDictionary<string, string> Labels { get; }

  internal MetricLineLabelsKey(SortedDictionary<string, string> labels, params string[] except)
  {
    Labels = new SortedDictionary<string, string>(
      labels ?? throw new ArgumentNullException(nameof(labels)));

    if (except is not null)
    {
      foreach (var item in except)
      {
        if (Labels.ContainsKey(item))
        {
          Labels.Remove(item);
        }
      }
    }
  }

  public override int GetHashCode() => DictionaryUtils.GetHash(Labels);

  public override bool Equals(object obj)
  {
    if (obj is null) return false;
    if (obj is not MetricLineLabelsKey) return false;

    var other = (MetricLineLabelsKey)obj;

    return DictionaryUtils.EqualMaps(Labels, other.Labels);
  }
}
