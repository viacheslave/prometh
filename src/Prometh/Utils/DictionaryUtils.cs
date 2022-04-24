namespace Prometh;

internal static class DictionaryUtils
{
  internal static bool EqualMaps(IDictionary<string, string> left, IDictionary<string, string> right)
  {
    if (left.Count != right.Count)
    {
      return false;
    }

    foreach (var kvp in left)
    {
      right.TryGetValue(kvp.Key, out var value);
      
      if (value is null || !kvp.Value.Equals(value))
      {
        return false;
      }
    }

    return true;
  }

  internal static int GetHash(SortedDictionary<string, string> map)
  {
    unchecked
    {
      var hash = 17;

      foreach (var kvp in map)
      {
        hash = (hash * 23) + kvp.GetHashCode();
      }

      return hash;
    }
  }
}
