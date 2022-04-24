namespace Prometh;

public abstract class Metric
{
  public string Name { get; }

  public string Help { get; }

  public abstract MetricType Type { get; }

  protected internal Metric(string name, string help)
  {
    Name = name ?? throw new ArgumentNullException(nameof(name));
    Help = help;
  }
}
