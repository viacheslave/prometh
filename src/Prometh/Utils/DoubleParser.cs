namespace Prometh;

internal static class DoubleParser
{
  internal static bool TryParse(this string str, out double value)
  {
    if (str == "+Inf")
    {
      value = double.PositiveInfinity;
      return true;
    }

    if (str == "-Inf")
    {
      value = double.NegativeInfinity;
      return true;
    }

    var parsed = double.TryParse(str, out value);
    return parsed;
  }
}
