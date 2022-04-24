namespace Prometh;

internal static class StringUtils
{
  internal const string Sum = "sum";
  internal const string Count = "count";
  internal const string Bucket = "bucket";

  internal const string Quantile = "quantile";
  internal const string Le = "le";

  internal static string WithSum(this string str) => str.WithSuffix(Sum);

  internal static string WithCount(this string str) => str.WithSuffix(Count);

  internal static string WithBucket(this string str) => str.WithSuffix(Bucket);

  private static string WithSuffix(this string str, string suffix) => $"{str}_{suffix}";
}
