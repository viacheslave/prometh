namespace Prometh;

internal class MetricLineKey
{
  public string Name { get; }

  public SortedDictionary<string, string> Labels { get; }

  public MetricLineLabelsKey LabelsKey { get; }
  public MetricLineLabelsKey LabelsExceptQuantileKey { get; }
  public MetricLineLabelsKey LabelsExceptLeKey { get; }

  public bool HasQuantile { get; }
  public bool HasLe { get; }

  public MetricLineKey(string name, SortedDictionary<string, string> labels)
  {
    Name = name ?? throw new ArgumentNullException(nameof(name));
    Labels = labels ?? throw new ArgumentNullException(nameof(labels));

    LabelsKey = new MetricLineLabelsKey(Labels);
    LabelsExceptQuantileKey = new MetricLineLabelsKey(Labels, "quantile");
    LabelsExceptLeKey = new MetricLineLabelsKey(Labels, "le");

    HasQuantile = Labels.ContainsKey("quantile");
    HasLe = Labels.ContainsKey("le");
  }

  public override int GetHashCode()
  {
    unchecked
    {
      var hash = 17;
      hash = (hash * 23) + Name.GetHashCode();
      hash = (hash * 23) + DictionaryUtils.GetHash(Labels);

      return hash;
    }
  }

  public override bool Equals(object obj)
  {
    if (obj is null) return false;
    if (obj is not MetricLineKey) return false;

    var other = (MetricLineKey)obj;

    return Name.Equals(other.Name) && DictionaryUtils.EqualMaps(Labels, other.Labels);
  }

  public override string ToString() => $"{Name} ({Labels.Count} label(s))";
}
