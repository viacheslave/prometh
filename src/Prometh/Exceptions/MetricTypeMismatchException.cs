namespace Prometh;

public sealed class MetricTypeMismatchException : Exception
{
  public Metric Metric { get; }

  public MetricTypeMismatchException(Metric metric)
    : base("The metric's type is different than the expected one")
  {
    Metric = metric;
  }
}
