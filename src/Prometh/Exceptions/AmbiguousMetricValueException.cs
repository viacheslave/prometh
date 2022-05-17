namespace Prometh;

public sealed class AmbiguousMetricValueException : Exception
{
  public Metric Metric { get; }

  public AmbiguousMetricValueException(Metric metric)
    : base("Multiple values found for a given metric's filter")
  {
    Metric = metric;
  }
}
