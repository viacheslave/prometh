namespace Prometh;

internal interface IMetricSpecification
{
  bool Satisfies(Metric metric);
}
